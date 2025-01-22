using Testcontainers.PostgreSql;

namespace Heyer.BuildingBlocks.Tests.Fixtures;

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