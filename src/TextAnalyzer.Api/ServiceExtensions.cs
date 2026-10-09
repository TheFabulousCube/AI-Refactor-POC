using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using OllamaSharp;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text.Json;
using TextAnalyzer.Api.Services;
using TextAnalyzer.Api.Configuration;

namespace TextAnalyzer.Api;

public static class ServiceExtensions
{
    public const string ServiceSourceName = "AnalysisApi";
    public const string ChatClientSourceName = "Experimental.Microsoft.Extensions.AI";

    public static IServiceCollection AddTextAnalyzerServices(this IServiceCollection services)
    {
        services.AddScoped<ITextAnalysisService, TextAnalysisService>();
        services.AddScoped<ISentimentAnalysisService, SentimentAnalysisService>();

        return services;
    }

    public static IServiceCollection AddChatClient(this IServiceCollection services)
    {
        services.AddSingleton<IChatClient>(_ =>
        {
            IChatClient innerClient = new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.2");

            return new ChatClientBuilder(innerClient)
                .UseOpenTelemetry(sourceName: ChatClientSourceName)
                .Build();
        });

        return services;
    }

    public static IServiceCollection AddCustomTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(ServiceSourceName))
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSource(ServiceSourceName)
                    .AddSource(ChatClientSourceName)
                    .AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"));
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddMeter(ChatClientSourceName)
                    .AddOtlpExporter(options => options.Endpoint = new Uri("http://localhost:4317"));
            });

        return services;
    }
}

