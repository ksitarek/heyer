using Npgsql;

namespace Heyer.BuildingBlocks.Tests.Fixtures;

public class LocalPostgresFixtureProvider : IPostgresFixtureProvider
{
    private readonly NpgsqlConnection _connection =
        new("Host=localhost;Port=5432;Database=postgres;TrustServerCertificate=True");

    private readonly string _dbName = $"TEST_{Guid.CreateVersion7()}";

    public string ConnectionString { get; private set; } = "";

    public async Task DisposeAsync()
    {
        await DropTestDatabase();

        await _connection.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        await CreateTestDatabase();

        ConnectionString = $"Host=localhost;Port=5432;Database={_dbName};TrustServerCertificate=True";
    }

    private async Task CreateTestDatabase()
    {
        var createDatabaseCommand = $@"CREATE DATABASE ""{_dbName}"";";

        await _connection.OpenAsync();

        await using var createCommand = new NpgsqlCommand(
            createDatabaseCommand,
            _connection);

        await createCommand.ExecuteNonQueryAsync();

        await _connection.CloseAsync();
    }

    private async Task DropTestDatabase()
    {
        var dropDatabaseCommand = $@"DROP DATABASE ""{_dbName}"" WITH (FORCE);";

        await _connection.OpenAsync();

        await using var dropCommand = new NpgsqlCommand(
            dropDatabaseCommand,
            _connection);

        await dropCommand.ExecuteNonQueryAsync();

        await _connection.CloseAsync();
    }
}