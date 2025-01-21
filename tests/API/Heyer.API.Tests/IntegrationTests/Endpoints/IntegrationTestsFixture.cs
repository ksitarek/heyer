using System.Text.RegularExpressions;
using Heyer.BuildingBlocks.Tests;
using Heyer.BuildingBlocks.Tests.Fixtures;
using Heyer.Meta.DbMigrator;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[SetUpFixture]
public class IntegrationTestsFixture
{
    private readonly Migrator _migrator = new();
    private readonly PostgresFixture _npgsqlFixture = new();

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await _npgsqlFixture.InitializeAsync();

        ApplicationFactoryConfiguration.AddConfig(
            Config.Npgsql_ConnectionString,
            GetConnectionString("heyer"));

        ApplicationFactoryConfiguration.AddConfig(
            Config.Scheduler_Npgsql_ConnectionString,
            GetConnectionString("scheduler"));

        ConfigureClientDb(ApplicationFactoryConfiguration.Client1Id);
        ConfigureClientDb(ApplicationFactoryConfiguration.Client2Id);

        _migrator.Migrate("JobBoardContext",
                          "job_board",
                          ApplicationFactoryConfiguration.InMemoryConfiguration[Config.Npgsql_ConnectionString]!);

        _migrator.Migrate("SchedulerDb",
                          ApplicationFactoryConfiguration.InMemoryConfiguration[
                              Config.Scheduler_Npgsql_ConnectionString]!);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _npgsqlFixture.DisposeAsync();

    private void ConfigureClientDb(Guid clientId)
    {
        var connectionString = GetConnectionString($"TEST_{Guid.NewGuid()}");

        ApplicationFactoryConfiguration.AddClientConfig(
            clientId,
            Config.Npgsql_ConnectionString,
            connectionString);

        _migrator.Migrate("HiringContext", connectionString);
    }

    private string GetConnectionString(string databaseName) =>
        Regex.Replace(_npgsqlFixture.ConnectionString,
                      "Database=(.*?)",
                      $"Database={databaseName}");
}