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

        ApplicationFactoryConfiguration.InMemoryConfiguration[Config.MongoDb_ConnectionString] =
            _mongoDbFixture.ConnectionString;

        ApplicationFactoryConfiguration.InMemoryConfiguration[
                "Companies:A62C048C-8E0F-41E2-84D4-BD061F9DDE97:MongoDb:ConnectionString"] =
            _mongoDbFixture.ConnectionString;
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _mongoDbFixture.DisposeAsync();
}