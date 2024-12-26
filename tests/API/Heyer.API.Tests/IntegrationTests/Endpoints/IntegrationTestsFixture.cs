using Heyer.BuildingBlocks.Tests;
using Heyer.BuildingBlocks.Tests.Fixtures;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[SetUpFixture]
public class IntegrationTestsFixture
{
    private readonly MongoDbFixture _mongoDbFixture = new();

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await _mongoDbFixture.InitializeAsync();

        ApplicationFactoryConfiguration.AddConfig(
            Config.MongoDb_ConnectionString,
            _mongoDbFixture.ConnectionString);

        ApplicationFactoryConfiguration.AddConfig(
            Config.Scheduler_MongoDb_ConnectionString,
            _mongoDbFixture.ConnectionString);

        ApplicationFactoryConfiguration.AddConfig(
            Config.HiringModule_InboxOutbox_MongoDb_ConnectionString,
            _mongoDbFixture.ConnectionString);

        ConfigureTenantDb(ApplicationFactoryConfiguration.Tenant1Id);
        ConfigureTenantDb(ApplicationFactoryConfiguration.Tenant2Id);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _mongoDbFixture.DisposeAsync();

    private void ConfigureTenantDb(Guid tenantId)
    {
        ApplicationFactoryConfiguration.AddTenantConfig(
            tenantId,
            Config.MongoDb_ConnectionString,
            _mongoDbFixture.ConnectionString);

        ApplicationFactoryConfiguration.AddTenantConfig(
            tenantId,
            Config.MongoDb_DatabaseName,
            tenantId.ToString());
    }
}