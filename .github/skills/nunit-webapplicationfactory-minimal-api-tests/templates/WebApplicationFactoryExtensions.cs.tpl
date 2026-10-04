using Microsoft.Extensions.DependencyInjection;

namespace {{TestRootNamespace}}.Infrastructure;

public static class WebApplicationFactoryExtensions
{
    public static async Task SeedAsync<TService>(
        this CustomWebApplicationFactory factory,
        Func<TService, Task> seed)
        where TService : notnull
    {
        using var scope = factory.Services.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<TService>();

        await seed(service);
    }

    public static TService GetRequiredService<TService>(
        this CustomWebApplicationFactory factory)
        where TService : notnull
    {
        using var scope = factory.Services.CreateScope();

        return scope.ServiceProvider.GetRequiredService<TService>();
    }
}