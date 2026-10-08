namespace TextAnalyzer.Api.Models;

public record AnalyzeResponse
{
    /// <summary>
    /// Number of words in the text
    /// </summary>
    public int WordCount { get; set; }

    /// <summary>
    /// Number of characters in the text
    /// </summary>
    public int CharacterCount { get; set; }
}
