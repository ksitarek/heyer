using Heyer.BuildingBlocks.Tests.Fixtures;

namespace Heyer.Modules.Hiring.Infrastructure.Tests.Persistence;

[SetUpFixture]
public class PersistenceTestsFixture
{
    private static readonly MongoDbFixture _mongoDbFixture = new();
    public static string ConnectionString => _mongoDbFixture.ConnectionString;

    [OneTimeSetUp]
    public static async Task OneTimeSetUp() => await _mongoDbFixture.InitializeAsync();


    [OneTimeTearDown]
    public static async Task OneTimeTearDown() => await _mongoDbFixture.DisposeAsync();
}