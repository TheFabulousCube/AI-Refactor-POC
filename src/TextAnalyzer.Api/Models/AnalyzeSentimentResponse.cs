using TextAnalyzer.Api.Configuration;

namespace TextAnalyzer.Api.Models;

public record AnalyzeSentimentResponse
{
    /// <summary>
    /// Sentiment score: Positive, Negative, or Neutral
    /// </summary>
    public SentimentResult Sentiment { get; set; }
}
