using System.Diagnostics;
using Npgsql;
using Testcontainers.PostgreSql;

namespace Heyer.BuildingBlocks.Tests.Fixtures;

public class PostgresFixture
{
    private readonly IPostgresFixtureProvider _provider;

    public PostgresFixture()
    {
        var dockerIsRunning = CheckForDocker();

        _provider = dockerIsRunning
            ? new DockerFixtureProvider()
            : new LocalPostgresFixtureProvider();
    }

    public string ConnectionString => _provider.ConnectionString;

    public async Task DisposeAsync() => await _provider.DisposeAsync();

    public async Task InitializeAsync() => await _provider.InitializeAsync();

    private bool CheckForDocker()
    {
        var processInfo = new ProcessStartInfo("docker", "ps");
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        processInfo.RedirectStandardOutput = true;
        processInfo.RedirectStandardError = true;

        int exitCode;
        using (var process = new Process())
        {
            process.StartInfo = processInfo;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit(1200000);
            if (!process.HasExited)
            {
                process.Kill();
            }

            exitCode = process.ExitCode;
            process.Close();
        }

        return exitCode == 0;
    }
}

public class LocalPostgresFixtureProvider : IPostgresFixtureProvider
{
    private readonly NpgsqlConnection _connection =
        new("Host=localhost;Port=5432;Database=postgres;TrustServerCertificate=True");

    private readonly string _dbName = $"TEST_{Guid.NewGuid()}";

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

public class DockerFixtureProvider : IPostgresFixtureProvider
{
    private readonly PostgreSqlContainer _npgsqlContainer;

    public DockerFixtureProvider()
    {
        var port = Random.Shared.Next(21433, 31433);

        _npgsqlContainer = new PostgreSqlBuilder()
            .WithPortBinding(port, 5432)
            .Build();
    }

    public string ConnectionString => _npgsqlContainer.GetConnectionString();

    public async Task DisposeAsync() => await _npgsqlContainer.StopAsync();

    public async Task InitializeAsync() => await _npgsqlContainer.StartAsync();
}

public interface IPostgresFixtureProvider
{
    string ConnectionString { get; }
    Task DisposeAsync();
    Task InitializeAsync();
}