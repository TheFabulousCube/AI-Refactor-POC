using NUnit.Framework;

namespace {{TestRootNamespace}}.Infrastructure;

public abstract class ApiTestFixture
{
    protected CustomWebApplicationFactory Factory = null!;
    protected HttpClient Client = null!;

    [SetUp]
    public virtual void SetUp()
    {
        Factory = new CustomWebApplicationFactory();

        Client = Factory.CreateClient(new()
        {
            AllowAutoRedirect = false
        });
    }

    [TearDown]
    public virtual void TearDown()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}
