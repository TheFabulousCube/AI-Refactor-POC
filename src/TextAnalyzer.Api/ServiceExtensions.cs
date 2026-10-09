using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using TextAnalyzer.Api.Services;

namespace TextAnalyzer.Api;

public static class ServiceExtensions
{
    public const string ServiceSourceName = "Custom.Application.Services";

    public static IServiceCollection AddTextAnalyzerServices(this IServiceCollection services)
    {
        services.AddScoped<ITextAnalysisService, TextAnalysisService>();
        services.AddScoped<ISentimentAnalysisService, SentimentAnalysisService>();

        return services;
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
