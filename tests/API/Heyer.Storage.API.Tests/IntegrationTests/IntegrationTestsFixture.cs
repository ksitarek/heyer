using Heyer.Storage.API.Tests.IntegrationTests.Fixtures;

namespace Heyer.Storage.API.Tests.IntegrationTests;

[SetUpFixture]
public class IntegrationTestsFixture
{
    private static readonly MongoDbFixture MongoDbFixture = new();
    
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await MongoDbFixture.InitializeAsync();

        ApplicationFactory.InMemoryConfiguration[Config.RegistryStrategy_MongoDbRegistry_ConnectionString]
            = MongoDbFixture.ConnectionString;
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await MongoDbFixture.DisposeAsync();
    }
}