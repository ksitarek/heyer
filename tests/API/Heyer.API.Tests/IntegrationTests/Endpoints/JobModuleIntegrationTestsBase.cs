using Heyer.API.Client;
using Heyer.BuildingBlocks.Tests;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

public abstract class JobModuleIntegrationTestsBase
{
    internal IApplicationFactory<IApiClient> AppFactory;

    [SetUp]
    public Task SetUp()
    {
        AppFactory = ApplicationFactory.Create();
        return Task.CompletedTask;
    }
    
    [TearDown]
    public async Task TearDown()
    {
        await AppFactory.DisposeAsync();
    }
}