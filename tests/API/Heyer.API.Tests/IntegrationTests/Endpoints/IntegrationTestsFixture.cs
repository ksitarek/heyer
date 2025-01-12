using Heyer.BuildingBlocks.Tests;
using Heyer.BuildingBlocks.Tests.Fixtures;
using Heyer.Meta.DbMigrator;
using Npgsql;

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

        var databases = new[]
        {
            "Heyer", "Scheduler", "HiringInboxOutbox", $"C_{ApplicationFactoryConfiguration.Client1Id}",
            $"C_{ApplicationFactoryConfiguration.Client2Id}"
        };

        // await CreateDatabases(databases);

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
        var connectionString = GetConnectionString($"C_{clientId}");

        ApplicationFactoryConfiguration.AddClientConfig(
            clientId,
            Config.Npgsql_ConnectionString,
            connectionString);

        _migrator.Migrate("HiringContext", connectionString);
    }

    private async Task CreateDatabases(string[] databases)
    {
        await using var connection = new NpgsqlConnection(_npgsqlFixture.ConnectionString);
        await connection.OpenAsync();

        foreach (var dbName in databases)
        {
            // Open a connection to the "postgres" database or any other system database
            var createDbCommand = $"SELECT 1 FROM pg_database WHERE datname = '{dbName}';";

            await using var checkDbCommand = new NpgsqlCommand(createDbCommand, connection);
            var dbExists = await checkDbCommand.ExecuteScalarAsync();

            // If the database doesn't exist, create it
            if (dbExists == null)
            {
                var createDatabaseCommand = $@"CREATE DATABASE ""{dbName}"";";

                await using var createCommand = new NpgsqlCommand(createDatabaseCommand, connection);
                await createCommand.ExecuteNonQueryAsync();
            }
        }

        await connection.CloseAsync();
    }

    private string GetConnectionString(string databaseName) =>
        _npgsqlFixture.ConnectionString.Replace("Database=postgres", $"Database={databaseName}");
}