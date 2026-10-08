using TextAnalyzer.Api.Models;

namespace TextAnalyzer.Api.Services;

public class TextAnalysisService : ITextAnalysisService
{
    public AnalyzeResponse AnalyzeText(string? text)
    {
        var wordCount = string.IsNullOrEmpty(text) ? 0 : text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        var characterCount = text?.Length ?? 0;

        return new AnalyzeResponse
        {
            WordCount = wordCount,
            CharacterCount = characterCount
        };
    }
}
