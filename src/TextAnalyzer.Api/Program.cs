var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Analyze text endpoint
app.MapPost("/tfc-analyze", (AnalyzeRequest request) =>
{
    var wordCount = string.IsNullOrEmpty(request.Text) ? 0 : request.Text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
    var characterCount = request.Text?.Length ?? 0;

    return Results.Ok(new AnalyzeResponse
    {
        WordCount = wordCount,
        CharacterCount = characterCount
    });
})
.WithName("AnalyzeText")
.WithOpenApi(operation =>
{
    operation.Summary = "Analyze text and return word count and character count";
    operation.Description = "Accepts a JSON body with a 'text' field and returns the word count and character count of that text.";
    return operation;
});

// Sentiment analysis endpoint
app.MapPost("/tfc-sentiment", (AnalyzeSentimentRequest request) =>
{
    var positiveWords = new[] { "good", "great", "excellent", "amazing", "awesome" };
    var negativeWords = new[] { "bad", "poor", "terrible", "awful", "horrible" };

    var positiveCount = request.Text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
        .Where(word => positiveWords.Contains(word.ToLower())).Count();

    var negativeCount = request.Text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
        .Where(word => negativeWords.Contains(word.ToLower())).Count();

    var neutral = !positiveWords.Any(word => request.Text.Contains(word)) && !negativeWords.Any(word => request.Text.Contains(word));

    return Results.Ok(new AnalyzeSentimentResponse
    {
        Sentiment = positiveCount > negativeCount ? "Positive" : (negativeCount > positiveCount ? "Negative" : "Neutral")
    });
})
.WithName("AnalyzeSentiment")
.WithOpenApi(operation =>
{
    operation.Summary = "Analyze text sentiment and return positive/negative/neutral score";
    operation.Description = "Accepts a JSON body with a 'text' field and returns the sentiment score of that text.";
    return operation;
});

app.Run();

public record AnalyzeRequest
{
    /// <summary>
    /// Text to analyze
    /// </summary>
    public required string Text { get; set; }
}

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

public record AnalyzeSentimentRequest
{
    /// <summary>
    /// Text to analyze for sentiment
    /// </summary>
    public required string Text { get; set; }
}

public record AnalyzeSentimentResponse
{
    /// <summary>
    /// Sentiment score: Positive, Negative, or Neutral
    /// </summary>
    public string Sentiment { get; set; }
}
