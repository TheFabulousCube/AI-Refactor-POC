using System.Net;
using System.Net.Http.Json;
using NUnit.Framework;
using TextAnalyzer.Api.Tests.Infrastructure;

namespace TextAnalyzer.Api.Tests.Endpoints;

[TestFixture]
public sealed class HealthTests : ApiTestFixture
{
    [Test]
    public async Task PostAnalyze_WhenCalled_ReturnsOk()
    {
        var response = await Client.PostAsJsonAsync("/tfc-analyze", new { Text = "hello world" });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task PostAnalyze_WhenCalled_ReturnsExpectedPayload()
    {
        var response = await Client.PostAsJsonAsync("/tfc-analyze", new { Text = "hello world" });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var body = await response.Content.ReadFromJsonAsync<AnalyzeResponse>();

        Assert.That(body, Is.Not.Null);
        Assert.That(body!.WordCount, Is.EqualTo(2));
        Assert.That(body!.CharacterCount, Is.EqualTo(11));
    }

    private sealed record AnalyzeResponse(int WordCount, int CharacterCount);
}
