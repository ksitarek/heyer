using Heyer.BuildingBlocks.Tests;
using Heyer.BuildingBlocks.Tests.Fixtures;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[SetUpFixture]
public class JobBoardIntegrationTestsFixture
{
    private readonly MongoDbFixture _mongoDbFixture = new();

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await _mongoDbFixture.InitializeAsync();

        ApplicationFactoryConfiguration.AddConfig(
            Config.MongoDb_ConnectionString,
            _mongoDbFixture.ConnectionString);

        ApplicationFactoryConfiguration.AddTenantConfig(
            ApplicationFactoryConfiguration.Tenant1Id,
            Config.MongoDb_ConnectionString,
            _mongoDbFixture.ConnectionString);

        ApplicationFactoryConfiguration.AddTenantConfig(
            ApplicationFactoryConfiguration.Tenant1Id,
            Config.MongoDb_DatabaseName,
            ApplicationFactoryConfiguration.Tenant1Id.ToString());

        ApplicationFactoryConfiguration.AddTenantConfig(
            ApplicationFactoryConfiguration.Tenant2Id,
            Config.MongoDb_ConnectionString,
            ApplicationFactoryConfiguration.Tenant2Id.ToString());
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _mongoDbFixture.DisposeAsync();
}