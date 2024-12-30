using Testcontainers.SqlEdge;

namespace Heyer.BuildingBlocks.Tests.Fixtures;

public class SqlEdgeFixture
{
    private readonly SqlEdgeContainer _sqlServerContainer =
        new SqlEdgeBuilder().Build();

    public string ConnectionString => _sqlServerContainer.GetConnectionString();

    public async Task DisposeAsync() => await _sqlServerContainer.StopAsync();

    public async Task InitializeAsync() => await _sqlServerContainer.StartAsync();
}