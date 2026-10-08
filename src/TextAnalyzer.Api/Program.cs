using TextAnalyzer.Api.Models;
using TextAnalyzer.Api.Services;
using TextAnalyzer.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddTextAnalyzerServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Analyze text endpoint
app.MapPost("/tfc-analyze", (AnalyzeRequest request, ITextAnalysisService textAnalysisService) =>
{
    return Results.Ok(textAnalysisService.AnalyzeText(request.Text));
});

// Sentiment analysis endpoint
app.MapPost("/tfc-sentiment", (AnalyzeSentimentRequest request, ISentimentAnalysisService sentimentAnalysisService) =>
{
    if (request == null || string.IsNullOrEmpty(request.Text)) {
          return Results.BadRequest(new { Error = "Request body is required" });
      }

    return Results.Ok(sentimentAnalysisService.AnalyzeSentiment(request.Text));
});

app.Run();
