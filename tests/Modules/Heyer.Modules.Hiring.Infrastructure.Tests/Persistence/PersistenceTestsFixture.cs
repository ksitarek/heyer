using Heyer.BuildingBlocks.Tests.Fixtures;

namespace Heyer.Modules.Hiring.Infrastructure.Tests.Persistence;

[SetUpFixture]
public class PersistenceTestsFixture
{
    private static readonly PostgresFixture _sqlEdgeFixture = new();
    public static string ConnectionString => _sqlEdgeFixture.ConnectionString;

    [OneTimeSetUp]
    public static async Task OneTimeSetUp() => await _sqlEdgeFixture.InitializeAsync();


    [OneTimeTearDown]
    public static async Task OneTimeTearDown() => await _sqlEdgeFixture.DisposeAsync();
}