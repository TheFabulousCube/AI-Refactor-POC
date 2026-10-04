var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

/// <summary>
/// Analyzes text content by counting words and characters
/// </summary>
/// <param name="request">The analysis request containing text input</param>
/// <returns>A response with word and character counts</returns>
app.MapPost("/tfc-analyze", (AnalyzeRequest request) =>
{
    if (request is null)
    {
        return Results.BadRequest(new { error = "Request body is required." });
    }

    if (request.Text is null || request.Text == string.Empty)
    {
        return Results.BadRequest(new { error = "The 'text' field is required." });
    }

    var wordCount = request.Text
        .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
        .Length;

    return Results.Ok(new AnalyzeResponse(wordCount, request.Text.Length));
});

/// <summary>
/// Evaluates the sentiment of text content and returns a basic sentiment score (Positive, Negative, Neutral)
/// </summary>
/// <param name="request">The sentiment analysis request containing text input</param>
/// <returns>A response with sentiment score</returns>
app.MapPost("/tfc-sentiment", (SentimentRequest request) =>
{
    if (request is null)
    {
        return Results.BadRequest(new { error = "Request body is required." });
    }

    if (request.Text is null || request.Text == string.Empty)
    {
        return Results.BadRequest(new { error = "The 'text' field is required." });
    }

    // Basic sentiment analysis using keyword matching
    var text = request.Text.ToLower();
    var positiveWords = new[] { "good", "great", "excellent", "amazing", "wonderful", "fantastic", "love", "like" };
    var negativeWords = new[] { "bad", "terrible", "awful", "horrible", "hate", "dislike", "worst", "not good" };

    var positiveCount = positiveWords.Count(word => text.Contains(word));
    var negativeCount = negativeWords.Count(word => text.Contains(word));

    string sentiment;
    if (positiveCount > negativeCount)
        sentiment = "Positive";
    else if (negativeCount > positiveCount)
        sentiment = "Negative";
    else
        sentiment = "Neutral";

    return Results.Ok(new SentimentResponse(sentiment));
});

app.Run();

record AnalyzeRequest(string? Text);

record AnalyzeResponse(int WordCount, int CharacterCount);

record SentimentRequest(string? Text);

record SentimentResponse(string Score);

public partial class Program { }
