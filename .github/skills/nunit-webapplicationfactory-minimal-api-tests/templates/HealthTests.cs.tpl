using System.Net;
using System.Net.Http.Json;
using NUnit.Framework;
using {{TestRootNamespace}}.Infrastructure;

namespace {{TestRootNamespace}}.Endpoints;

[TestFixture]
public sealed class HealthTests : ApiTestFixture
{
    [Test]
    public async Task GetHealth_WhenCalled_ReturnsOk()
    {
        var response = await Client.GetAsync("{{HealthEndpoint}}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetHealth_WhenCalled_ReturnsExpectedPayload()
    {
        var response = await Client.GetAsync("{{HealthEndpoint}}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var body = await response.Content.ReadFromJsonAsync<HealthResponse>();

        Assert.That(body, Is.Not.Null);
        Assert.That(body!.Status, Is.EqualTo("ok"));
    }

    private sealed record HealthResponse(string Status);
}
