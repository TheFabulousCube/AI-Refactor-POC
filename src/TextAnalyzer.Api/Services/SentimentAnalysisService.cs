using System.Diagnostics;
using TextAnalyzer.Api.Models;

namespace TextAnalyzer.Api.Services;

public class SentimentAnalysisService : ISentimentAnalysisService
{
    private static readonly ActivitySource _activitySource = new(TextAnalyzer.Api.ServiceExtensions.ServiceSourceName);

    public AnalyzeSentimentResponse AnalyzeSentiment(string? text)
    {
        using var activity = _activitySource.StartActivity("AnalyzeSentimentExecution");
        activity?.SetTag("custom.text.length", text?.Length ?? 0);

        if (string.IsNullOrEmpty(text))
        {
            return new AnalyzeSentimentResponse
            {
                Sentiment = SentimentResult.Neutral
            };
        }

        var positiveWords = new[] { "good", "great", "excellent", "amazing", "awesome" };
        var negativeWords = new[] { "bad", "poor", "terrible", "awful", "horrible" };

        var positiveCount = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Count(word => positiveWords.Contains(word.ToLower()));

        var negativeCount = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Count(word => negativeWords.Contains(word.ToLower()));

        return new AnalyzeSentimentResponse
        {
            Sentiment = positiveCount > negativeCount ? SentimentResult.Positive : (negativeCount > positiveCount ? SentimentResult.Negative : SentimentResult.Neutral)
        };
    }
}
