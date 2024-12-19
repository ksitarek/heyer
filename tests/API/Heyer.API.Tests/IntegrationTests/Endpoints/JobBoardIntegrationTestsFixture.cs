using Heyer.BuildingBlocks.Tests;
using Heyer.BuildingBlocks.Tests.Fixtures;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[SetUpFixture]
public class JobBoardIntegrationTestsFixture
{
    private MongoDbFixture _mongoDbFixture = new();
    
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await _mongoDbFixture.InitializeAsync();
    
        ApplicationFactoryConfiguration.InMemoryConfiguration[Config.MongoDb_ConnectionString] = _mongoDbFixture.ConnectionString;
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _mongoDbFixture.DisposeAsync();
    }
}