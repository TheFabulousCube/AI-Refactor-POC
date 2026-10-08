using TextAnalyzer.Api.Models;

namespace TextAnalyzer.Api.Services;

public interface ITextAnalysisService
{
    AnalyzeResponse AnalyzeText(string? text);
}

public interface ISentimentAnalysisService
{
    AnalyzeSentimentResponse AnalyzeSentiment(string? text);
}
