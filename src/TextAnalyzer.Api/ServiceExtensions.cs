using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using OllamaSharp;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text.Json;
using TextAnalyzer.Api.Services;

namespace TextAnalyzer.Api;

public static class ServiceExtensions
{
    public const string ServiceSourceName = "Custom.Application.Services";

    public static IServiceCollection AddTextAnalyzerServices(this IServiceCollection services)
    {
        services.AddScoped<ITextAnalysisService, TextAnalysisService>();
        services.AddScoped<ISentimentAnalysisService, SentimentAnalysisService>();

        services.AddSingleton<IChatClient>(_ =>
        {
            IChatClient innerClient = new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.2");

            return new ChatClientBuilder(innerClient)
                .UseOpenTelemetry(sourceName: "Experimental.Microsoft.Extensions.AI")
                .Build();
        });

        return services;
    }

    public static ChatOptions GetSentimentChatOptions()
    {
        var schema = JsonSerializer.Serialize(new
        {
            type = "object",
            properties = new
            {
                sentiment = new
                {
                    type = "string",
                    @enum = new[] { "Positive", "Neutral", "Negative" }
                }
            },
            required = new[] { "sentiment" }
        });

        return new ChatOptions
        {
            ResponseFormat = ChatResponseFormat.Json,
            AdditionalProperties = new AdditionalPropertiesDictionary { ["json_schema"] = schema }
        };
    }

    public static IServiceCollection AddCustomTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("SentimentAnalysisBaselineApi"))
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource(ServiceSourceName)
                    .AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"));
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"));
            });

        return services;
    }
}

