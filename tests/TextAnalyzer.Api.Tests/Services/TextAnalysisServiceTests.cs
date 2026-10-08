using TextAnalyzer.Api.Services;
using Xunit;

namespace TextAnalyzer.Api.Tests.Services;

public class TextAnalysisServiceTests
{
    private readonly TextAnalysisService _service = new();

    [Fact]
    public void AnalyzeText_WithNullInput_ReturnsZeroCounts()
    {
        // Arrange
        string? text = null;

        // Act
        var response = _service.AnalyzeText(text);

        // Assert
        Assert.Equal(0, response.WordCount);
        Assert.Equal(0, response.CharacterCount);
    }

    [Fact]
    public void AnalyzeText_WithEmptyInput_ReturnsZeroCounts()
    {
        // Arrange
        string text = string.Empty;

        // Act
        var response = _service.AnalyzeText(text);

        // Assert
        Assert.Equal(0, response.WordCount);
        Assert.Equal(0, response.CharacterCount);
    }

    [Fact]
    public void AnalyzeText_WithSingleWord_ReturnsCorrectCounts()
    {
        // Arrange
        string text = "Hello";

        // Act
        var response = _service.AnalyzeText(text);

        // Assert
        Assert.Equal(1, response.WordCount);
        Assert.Equal(5, response.CharacterCount);
    }

    [Theory]
    [InlineData("Hello world", 2, 11)]
    [InlineData("Hello\tworld\nnew\rline", 4, 20)]
    [InlineData("   spaces   ", 1, 12)]
    [InlineData("\t\n\r ", 0, 4)]
    [InlineData("!@#$%^&*()", 1, 10)]
    [InlineData("😊", 1, 2)]
    public void AnalyzeText_WithVariousInputs_ReturnsCorrectCounts(string text, int expectedWordCount, int expectedCharCount)
    {
        // Act
        var response = _service.AnalyzeText(text);

        // Assert
        Assert.Equal(expectedWordCount, response.WordCount);
        Assert.Equal(expectedCharCount, response.CharacterCount);
    }
}
