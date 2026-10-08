using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;
using TextAnalyzer.Api.Models;
using Xunit;

namespace TextAnalyzer.Api.Tests;

public class EndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public EndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    #region AnalyzeText Endpoint Tests

    [Fact]
    public async Task AnalyzeTextEndpoint_ReturnsCorrectWordAndCharacterCount()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new AnalyzeRequest { Text = "Hello world this is a test" };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/tfc-analyze", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<AnalyzeResponse>(responseContent, _jsonOptions);
        Assert.Equal(6, responseObject?.WordCount);
        Assert.Equal(26, responseObject?.CharacterCount);
    }

    [Theory]
    [InlineData("Hello world")]
    [InlineData("")]
    public async Task AnalyzeTextEndpoint_HandlesVariousInputs(string inputText)
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new AnalyzeRequest { Text = inputText };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/tfc-analyze", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AnalyzeTextEndpoint_HandlesEmptyString()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new AnalyzeRequest { Text = "" };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/tfc-analyze", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<AnalyzeResponse>(responseContent, _jsonOptions);
        Assert.Equal(0, responseObject?.WordCount);
        Assert.Equal(0, responseObject?.CharacterCount);
    }

    [Fact]
    public async Task AnalyzeTextEndpoint_HandlesWhitespaceOnlyString()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new AnalyzeRequest { Text = "   \t\n  " };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/tfc-analyze", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<AnalyzeResponse>(responseContent, _jsonOptions);
        Assert.Equal(0, responseObject?.WordCount);
        Assert.Equal(7, responseObject?.CharacterCount);
    }

    [Fact]
    public async Task AnalyzeTextEndpoint_HandlesSpecialCharacters()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new AnalyzeRequest { Text = "Hello, world! How are you? @#$%^&*()" };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/tfc-analyze", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<AnalyzeResponse>(responseContent, _jsonOptions);
        Assert.Equal(6, responseObject?.WordCount);  // Counted words without special characters as separate words
        Assert.Equal(36, responseObject?.CharacterCount);
    }

    [Fact]
    public async Task AnalyzeTextEndpoint_HandlesBoundaryConditions()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Test single character
        var request1 = new AnalyzeRequest { Text = "a" };
        var json1 = JsonSerializer.Serialize(request1);
        var content1 = new StringContent(json1, Encoding.UTF8, "application/json");

        // Test large text
        var largeText = new string('x', 10000);
        var request2 = new AnalyzeRequest { Text = largeText };
        var json2 = JsonSerializer.Serialize(request2);
        var content2 = new StringContent(json2, Encoding.UTF8, "application/json");

        // Act and Assert for single character
        var response1 = await client.PostAsync("/tfc-analyze", content1);
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        var responseContent1 = await response1.Content.ReadAsStringAsync();
        var responseObject1 = JsonSerializer.Deserialize<AnalyzeResponse>(responseContent1, _jsonOptions);
        Assert.Equal(1, responseObject1?.WordCount);
        Assert.Equal(1, responseObject1?.CharacterCount);

        // Act and Assert for large text
        var response2 = await client.PostAsync("/tfc-analyze", content2);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        var responseContent2 = await response2.Content.ReadAsStringAsync();
        var responseObject2 = JsonSerializer.Deserialize<AnalyzeResponse>(responseContent2, _jsonOptions);
        Assert.Equal(10000, responseObject2?.CharacterCount);
    }

    [Fact]
    public async Task AnalyzeTextEndpoint_HandlesNullInput()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new AnalyzeRequest { Text = null };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/tfc-analyze", content);

        // Assert - should handle null gracefully
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<AnalyzeResponse>(responseContent, _jsonOptions);
        Assert.Equal(0, responseObject?.WordCount);
        Assert.Equal(0, responseObject?.CharacterCount);
    }

    #endregion

    #region AnalyzeSentiment Endpoint Tests

    [Fact]
    public async Task AnalyzeSentimentEndpoint_ReturnsPositiveSentiment()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new AnalyzeSentimentRequest { Text = "This is great and wonderful and amazing" };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/tfc-sentiment", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<AnalyzeSentimentResponse>(responseContent, _jsonOptions);
        Assert.Equal("Positive", responseObject?.Sentiment);
    }

    [Fact]
    public async Task AnalyzeSentimentEndpoint_ReturnsNegativeSentiment()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new AnalyzeSentimentRequest { Text = "This is bad and poor and terrible" };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/tfc-sentiment", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<AnalyzeSentimentResponse>(responseContent, _jsonOptions);
        Assert.Equal("Negative", responseObject?.Sentiment);
    }

    [Fact]
    public async Task AnalyzeSentimentEndpoint_ReturnsNeutralSentiment()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new AnalyzeSentimentRequest { Text = "This is just neutral text" };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/tfc-sentiment", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<AnalyzeSentimentResponse>(responseContent, _jsonOptions);
        Assert.Equal("Neutral", responseObject?.Sentiment);
    }

    [Fact]
    public async Task AnalyzeSentimentEndpoint_HandlesEdgeCases()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Test case with equal positive and negative words (should be Neutral)
        var request1 = new AnalyzeSentimentRequest { Text = "good bad excellent terrible" };
        var json1 = JsonSerializer.Serialize(request1);
        var content1 = new StringContent(json1, Encoding.UTF8, "application/json");

        // Test case with no sentiment words
        var request2 = new AnalyzeSentimentRequest { Text = "This is just some text without any sentiment" };
        var json2 = JsonSerializer.Serialize(request2);
        var content2 = new StringContent(json2, Encoding.UTF8, "application/json");

        // Test case with only positive words (should be Positive)
        var request3 = new AnalyzeSentimentRequest { Text = "excellent amazing wonderful great awesome" };
        var json3 = JsonSerializer.Serialize(request3);
        var content3 = new StringContent(json3, Encoding.UTF8, "application/json");

        // Test case with only negative words (should be Negative)
        var request4 = new AnalyzeSentimentRequest { Text = "terrible awful poor bad horrible" };
        var json4 = JsonSerializer.Serialize(request4);
        var content4 = new StringContent(json4, Encoding.UTF8, "application/json");

        // Act and Assert for equal positive and negative
        var response1 = await client.PostAsync("/tfc-sentiment", content1);
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        var responseContent1 = await response1.Content.ReadAsStringAsync();
        var responseObject1 = JsonSerializer.Deserialize<AnalyzeSentimentResponse>(responseContent1, _jsonOptions);
        Assert.Equal("Neutral", responseObject1?.Sentiment);

        // Act and Assert for no sentiment words
        var response2 = await client.PostAsync("/tfc-sentiment", content2);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        var responseContent2 = await response2.Content.ReadAsStringAsync();
        var responseObject2 = JsonSerializer.Deserialize<AnalyzeSentimentResponse>(responseContent2, _jsonOptions);
        Assert.Equal("Neutral", responseObject2?.Sentiment);

        // Act and Assert for only positive words
        var response3 = await client.PostAsync("/tfc-sentiment", content3);
        Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
        var responseContent3 = await response3.Content.ReadAsStringAsync();
        var responseObject3 = JsonSerializer.Deserialize<AnalyzeSentimentResponse>(responseContent3, _jsonOptions);
        Assert.Equal("Positive", responseObject3?.Sentiment);

        // Act and Assert for only negative words
        var response4 = await client.PostAsync("/tfc-sentiment", content4);
        Assert.Equal(HttpStatusCode.OK, response4.StatusCode);
        var responseContent4 = await response4.Content.ReadAsStringAsync();
        var responseObject4 = JsonSerializer.Deserialize<AnalyzeSentimentResponse>(responseContent4, _jsonOptions);
        Assert.Equal("Negative", responseObject4?.Sentiment);
    }

    [Fact]
    public async Task AnalyzeSentimentEndpoint_HandlesMultipleSpacesAndSpecialCharacters()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new AnalyzeSentimentRequest { Text = "   great   wonderful   \n\t  amazing  " };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/tfc-sentiment", content);

        // Assert - should be positive due to presence of positive words
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<AnalyzeSentimentResponse>(responseContent, _jsonOptions);
        Assert.Equal("Positive", responseObject?.Sentiment);
    }

    [Fact]
    public async Task AnalyzeSentimentEndpoint_HandlesBoundaryConditions()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Test with single word (should be Neutral if not in positive/negative list)
        var request1 = new AnalyzeSentimentRequest { Text = "word" };
        var json1 = JsonSerializer.Serialize(request1);
        var content1 = new StringContent(json1, Encoding.UTF8, "application/json");

        // Test with empty string
        var request2 = new AnalyzeSentimentRequest { Text = "" };
        var json2 = JsonSerializer.Serialize(request2);
        var content2 = new StringContent(json2, Encoding.UTF8, "application/json");

        // Test with whitespace only
        var request3 = new AnalyzeSentimentRequest { Text = "  \t\n  " };
        var json3 = JsonSerializer.Serialize(request3);
        var content3 = new StringContent(json3, Encoding.UTF8, "application/json");

        // Act and Assert for single word
        var response1 = await client.PostAsync("/tfc-sentiment", content1);
        Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
        var responseContent1 = await response1.Content.ReadAsStringAsync();
        var responseObject1 = JsonSerializer.Deserialize<AnalyzeSentimentResponse>(responseContent1, _jsonOptions);
        Assert.Equal("Neutral", responseObject1?.Sentiment);

        // Act and Assert for empty string
        var response2 = await client.PostAsync("/tfc-sentiment", content2);
        Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);

        // Act and Assert for whitespace only
        var response3 = await client.PostAsync("/tfc-sentiment", content3);
        Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
        var responseContent3 = await response3.Content.ReadAsStringAsync();
        var responseObject3 = JsonSerializer.Deserialize<AnalyzeSentimentResponse>(responseContent3, _jsonOptions);
        Assert.Equal("Neutral", responseObject3?.Sentiment);
    }

    [Fact]
    public async Task AnalyzeSentimentEndpoint_HandlesNullInput()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new AnalyzeSentimentRequest { Text = null };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/tfc-sentiment", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion
}