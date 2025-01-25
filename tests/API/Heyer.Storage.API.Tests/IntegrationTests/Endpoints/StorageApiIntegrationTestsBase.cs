using Heyer.BuildingBlocks.Tests;
using Heyer.Storage.API.Client;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

public abstract class StorageApiIntegrationTestsBase
{
    internal IApplicationFactory<IStorageApiClient> _appFactory = null!;

    [SetUp]
    public Task SetUp()
    {
        _appFactory = ApplicationFactory.Create();
        return Task.CompletedTask;
    }

    [TearDown]
    public async Task TearDown() => await _appFactory.DisposeAsync();
}