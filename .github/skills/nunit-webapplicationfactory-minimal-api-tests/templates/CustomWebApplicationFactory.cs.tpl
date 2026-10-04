using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace {{TestRootNamespace}}.Infrastructure;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ASPNETCORE_ENVIRONMENT"] = "Testing"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Replace production services here.
            //
            // Examples:
            // - Replace production database with test database
            // - Replace external API clients with fakes
            // - Replace message bus clients with in-memory fakes
            // - Configure test authentication
        });
    }
}