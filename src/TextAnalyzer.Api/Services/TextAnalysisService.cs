using System.Diagnostics;
using TextAnalyzer.Api.Models;

namespace TextAnalyzer.Api.Services;

public class TextAnalysisService : ITextAnalysisService
{
    private static readonly ActivitySource _activitySource = new(TextAnalyzer.Api.ServiceExtensions.ServiceSourceName);

    public AnalyzeResponse AnalyzeText(string? text)
    {
        using var activity = _activitySource.StartActivity("AnalyzeWordsExecution");

        var wordCount = string.IsNullOrEmpty(text) ? 0 : text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        var characterCount = text?.Length ?? 0;

        return new AnalyzeResponse
        {
            WordCount = wordCount,
            CharacterCount = characterCount
        };
    }
}
