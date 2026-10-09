using TextAnalyzer.Api.Models;

namespace TextAnalyzer.Api.Services;

public interface ISentimentAnalysisService
{
    AnalyzeSentimentResponse AnalyzeSentiment(string? text);
}
