namespace TextAnalyzer.Api.Models;

public record AnalyzeSentimentRequest
{
    /// <summary>
    /// Text to analyze for sentiment
    /// </summary>
    public required string Text { get; set; }
}
