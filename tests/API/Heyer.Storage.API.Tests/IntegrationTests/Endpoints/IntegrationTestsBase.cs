using Heyer.BuildingBlocks.Tests;
using Heyer.Storage.API.Client;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

public abstract class IntegrationTestsBase
{
    internal IApplicationFactory<IStorageApiClient> AppFactory;

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