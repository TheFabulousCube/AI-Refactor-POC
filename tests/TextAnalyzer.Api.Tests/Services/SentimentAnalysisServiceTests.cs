using TextAnalyzer.Api.Configuration;
using TextAnalyzer.Api.Models;
using TextAnalyzer.Api.Services;
using Xunit;

namespace SentimentAnalyzer.Api.Tests.Services;

public class SentimentAnalysisServiceTests
{
    private readonly SentimentAnalysisService _service = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void AnalyzeSentiment_WithNullOrEmptyInput_ReturnsNeutral(string? text)
    {
        // Act
        var response = _service.AnalyzeSentiment(text);

        // Assert
        Assert.Equal(SentimentResult.Neutral, response.Sentiment);
    }

    [Fact]
    public void AnalyzeSentiment_WithPositiveText_ReturnsPositive()
    {
        // Arrange
        string text = "This is a great and amazing day!";

        // Act
        var response = _service.AnalyzeSentiment(text);

        // Assert
        Assert.Equal(SentimentResult.Positive, response.Sentiment);
    }

    [Fact]
    public void AnalyzeSentiment_WithNegativeText_ReturnsNegative()
    {
        // Arrange
        string text = "This is a terrible and awful experience.";

        // Act
        var response = _service.AnalyzeSentiment(text);

        // Assert
        Assert.Equal(SentimentResult.Negative, response.Sentiment);
    }

    [Fact]
    public void AnalyzeSentiment_WithNeutralText_ReturnsNeutral()
    {
        // Arrange
        string text = "The weather is okay today.";

        // Act
        var response = _service.AnalyzeSentiment(text);

        // Assert
        Assert.Equal(SentimentResult.Neutral, response.Sentiment);
    }

    [Fact]
    public void AnalyzeSentiment_WithMorePositiveThanNegative_ReturnsPositive()
    {
        // Arrange
        string text = "It was good and great, but a bit bad.";

        // Act
        var response = _service.AnalyzeSentiment(text);

        // Assert
        Assert.Equal(SentimentResult.Positive, response.Sentiment);
    }

    [Fact]
    public void AnalyzeSentiment_WithMoreNegativeThanPositive_ReturnsNegative()
    {
        // Arrange
        string text = "It was bad and terrible, though somewhat good.";

        // Act
        var response = _service.AnalyzeSentiment(text);

        // Assert
        Assert.Equal(SentimentResult.Negative, response.Sentiment);
    }

    [Fact]
    public void AnalyzeSentiment_WithCaseInsensitivity_ReturnsCorrectSentiment()
    {
        // Arrange
        string text = "This is GREAT and AMAZING!";

        // Act
        var response = _service.AnalyzeSentiment(text);

        // Assert
        Assert.Equal(SentimentResult.Positive, response.Sentiment);
    }

    [Fact]
    public void AnalyzeSentiment_WithNonSentimentWords_ReturnsNeutral()
    {
        // Arrange
        string text = "The quick brown fox jumps over the lazy dog.";

        // Act
        var response = _service.AnalyzeSentiment(text);

        // Assert
        Assert.Equal(SentimentResult.Neutral, response.Sentiment);
    }

    [Fact]
    public void AnalyzeSentiment_WithWordsThatAreSubstrings_ReturnsNeutral()
    {
        // Arrange
        string text = "This is not badly."; // "badly" contains "bad" but should be neutral

        // Act
        var response = _service.AnalyzeSentiment(text);

        // Assert
        Assert.Equal(SentimentResult.Neutral, response.Sentiment);
    }

    [Fact]
    public void AnalyzeSentiment_WithNumbersAndPunctuation_ReturnsNeutral()
    {
        // Arrange
        string text = "123 !!! 456";

        // Act
        var response = _service.AnalyzeSentiment(text);

        // Assert
        Assert.Equal(SentimentResult.Neutral, response.Sentiment);
    }

    [Fact]
    public void AnalyzeSentiment_WithEqualPositiveAndNegative_ReturnsNeutral()
    {
        // Arrange
        string text = "It was good but bad";

        // Act
        var response = _service.AnalyzeSentiment(text);

        // Assert
        Assert.Equal(SentimentResult.Neutral, response.Sentiment);
    }
}



