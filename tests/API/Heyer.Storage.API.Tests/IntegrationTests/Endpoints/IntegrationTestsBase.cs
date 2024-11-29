namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

public abstract class IntegrationTestsBase
{
    internal IApplicationFactory AppFactory;

    [SetUp]
    public async Task SetUp()
    {
        AppFactory = ApplicationFactory.Create();
    }
    
    [TearDown]
    public async Task TearDown()
    {
        await AppFactory.DisposeAsync();
    }
}