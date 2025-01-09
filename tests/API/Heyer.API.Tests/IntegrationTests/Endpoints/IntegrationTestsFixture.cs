using Heyer.BuildingBlocks.Tests;
using Heyer.BuildingBlocks.Tests.Fixtures;
using Microsoft.Data.SqlClient;

namespace Heyer.API.Tests.IntegrationTests.Endpoints;

[SetUpFixture]
public class IntegrationTestsFixture
{
    private readonly SqlEdgeFixture _sqlEdgeFixture = new();

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await _sqlEdgeFixture.InitializeAsync();

        var databases = new[]
        {
            "Heyer", "Scheduler", "HiringInboxOutbox", ApplicationFactoryConfiguration.Client1Id.ToString(),
            ApplicationFactoryConfiguration.Client2Id.ToString()
        };

        await CreateDatabases(databases);

        ApplicationFactoryConfiguration.AddConfig(
            Config.SqlServer_ConnectionString,
            _sqlEdgeFixture.ConnectionString.Replace("master", "Heyer"));

        ApplicationFactoryConfiguration.AddConfig(
            Config.Scheduler_SqlServer_ConnectionString,
            _sqlEdgeFixture.ConnectionString.Replace("master", "Scheduler"));

        // ApplicationFactoryConfiguration.AddConfig(
        //     Config.HiringModule_InboxOutbox_SqlServer_ConnectionString,
        //     _sqlEdgeFixture.ConnectionString.Replace("master", "HiringInboxOutbox"));

        ConfigureClientDb(ApplicationFactoryConfiguration.Client1Id);
        ConfigureClientDb(ApplicationFactoryConfiguration.Client2Id);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _sqlEdgeFixture.DisposeAsync();

    private void ConfigureClientDb(Guid clientId) =>
        ApplicationFactoryConfiguration.AddClientConfig(
            clientId,
            Config.SqlServer_ConnectionString,
            _sqlEdgeFixture.ConnectionString.Replace("master", clientId.ToString()));

    private async Task CreateDatabases(string[] databases)
    {
        await using var connection = new SqlConnection(_sqlEdgeFixture.ConnectionString);
        await connection.OpenAsync();

        foreach (var dbName in databases)
        {
            var command =
                $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{dbName}') CREATE DATABASE [{dbName}];";

            await using var sqlCommand = new SqlCommand(command, connection);
            await sqlCommand.ExecuteNonQueryAsync();
        }

        await connection.CloseAsync();
    }
}