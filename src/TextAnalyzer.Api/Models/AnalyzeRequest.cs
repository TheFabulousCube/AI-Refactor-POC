namespace TextAnalyzer.Api.Models;

public record AnalyzeRequest
{
    /// <summary>
    /// Text to analyze
    /// </summary>
    public required string Text { get; set; }
}
