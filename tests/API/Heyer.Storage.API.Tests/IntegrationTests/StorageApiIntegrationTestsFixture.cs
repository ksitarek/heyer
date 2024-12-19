using Heyer.BuildingBlocks.Tests;
using Heyer.BuildingBlocks.Tests.Fixtures;

namespace Heyer.Storage.API.Tests.IntegrationTests;

[SetUpFixture]
public class StorageApiIntegrationTestsFixture
{
    private readonly MongoDbFixture _mongoDbFixture = new();

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await _mongoDbFixture.InitializeAsync();

        ApplicationFactoryConfiguration.InMemoryConfiguration[Config.RegistryStrategy_MongoDbRegistry_ConnectionString]
            = _mongoDbFixture.ConnectionString;
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _mongoDbFixture.DisposeAsync();
}