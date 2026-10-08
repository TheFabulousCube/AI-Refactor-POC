namespace TextAnalyzer.Api.Models;

public record AnalyzeSentimentResponse
{
    /// <summary>
    /// Sentiment score: Positive, Negative, or Neutral
    /// </summary>
    public string Sentiment { get; set; }
}
