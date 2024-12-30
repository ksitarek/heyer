using Heyer.BuildingBlocks.Tests;
using Heyer.BuildingBlocks.Tests.Fixtures;

namespace Heyer.Storage.API.Tests.IntegrationTests;

[SetUpFixture]
public class StorageApiIntegrationTestsFixture
{
    private readonly SqlEdgeFixture _sqlEdgeFixture = new();

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await _sqlEdgeFixture.InitializeAsync();

        ApplicationFactoryConfiguration.InMemoryConfiguration[
                Config.RegistryStrategy_SqlServerRegistry_ConnectionString]
            = _sqlEdgeFixture.ConnectionString;
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _sqlEdgeFixture.DisposeAsync();
}