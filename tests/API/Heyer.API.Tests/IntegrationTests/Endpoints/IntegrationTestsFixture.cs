using System.Text.RegularExpressions;
using Heyer.BuildingBlocks.Tests;
using Heyer.BuildingBlocks.Tests.Fixtures;
using Heyer.Meta.DbMigrator;
using Npgsql;
using Serilog;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[SetUpFixture]
public class IntegrationTestsFixture
{
    private readonly Migrator _migrator = new();
    private readonly PostgresFixture _npgsqlFixture = new();

    private readonly List<string> _testDatabases = new();

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
    public async Task OneTimeTearDown()
    {
        await using var connection = new NpgsqlConnection(_npgsqlFixture.ConnectionString);

        await connection.OpenAsync();

        foreach (var testDatabase in _testDatabases)
        {
            try
            {
                await DropTestDatabase(connection, testDatabase);
            }
            catch (Exception e)
            {
                Log.Error(e, "Drop db failed");
            }
        }

        await connection.CloseAsync();

        await _npgsqlFixture.DisposeAsync();
    }

    private void ConfigureClientDb(Guid clientId)
    {
        var connectionString = GetConnectionString($"C_{clientId}");

        ApplicationFactoryConfiguration.AddClientConfig(
            clientId,
            Config.Npgsql_ConnectionString,
            connectionString);

        _migrator.Migrate("HiringContext", connectionString);
    }

    private async Task DropTestDatabase(NpgsqlConnection connection, string dbName)
    {
        var dropDatabaseCommand = $@"DROP DATABASE ""{dbName}"" WITH (FORCE);";

        await using var dropCommand = new NpgsqlCommand(
            dropDatabaseCommand,
            connection);

        await dropCommand.ExecuteNonQueryAsync();
    }

    private string GetConnectionString(string databaseName)
    {
        databaseName += Guid.NewGuid();

        _testDatabases.Add(databaseName);

        return Regex.Replace(_npgsqlFixture.ConnectionString,
                             "Database=(.*?);",
                             $"Database={databaseName};");
    }
}