namespace TextAnalyzer.Api.Models;

public enum SentimentResult
{
    Positive,
    Negative,
    Neutral
}

public record AnalyzeSentimentResponse
{
    /// <summary>
    /// Sentiment score: Positive, Negative, or Neutral
    /// </summary>
    public SentimentResult Sentiment { get; set; }
}
