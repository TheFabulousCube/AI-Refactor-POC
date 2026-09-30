var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/analyze", (AnalyzeRequest request) =>
{
    if (request.Text is null)
    {
        return Results.BadRequest(new { error = "The 'text' field is required." });
    }

    var wordCount = request.Text
        .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
        .Length;

    return Results.Ok(new AnalyzeResponse(wordCount, request.Text.Length));
});

app.Run();

record AnalyzeRequest(string? Text);

record AnalyzeResponse(int WordCount, int CharacterCount);
