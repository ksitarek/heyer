using Testcontainers.SqlEdge;

namespace Heyer.BuildingBlocks.Tests.Fixtures;

public class SqlEdgeFixture
{
    private readonly SqlEdgeContainer _sqlServerContainer;

    public SqlEdgeFixture()
    {
        var port = Random.Shared.Next(21433, 31433);

        _sqlServerContainer = new SqlEdgeBuilder()
            .WithPortBinding(port, 1433)
            .Build();
    }

    public string ConnectionString => _sqlServerContainer.GetConnectionString();

    public async Task DisposeAsync() => await _sqlServerContainer.StopAsync();

    public async Task InitializeAsync() => await _sqlServerContainer.StartAsync();
}