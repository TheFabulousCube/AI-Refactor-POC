using Microsoft.Extensions.DependencyInjection;
using TextAnalyzer.Api.Services;

namespace TextAnalyzer.Api;

public static class ServiceExtensions
{
    public static IServiceCollection AddTextAnalyzerServices(this IServiceCollection services)
    {
        services.AddScoped<ITextAnalysisService, TextAnalysisService>();
        services.AddScoped<ISentimentAnalysisService, SentimentAnalysisService>();

        return services;
    }
}
