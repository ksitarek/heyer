using Testcontainers.MongoDb;

namespace Heyer.BuildingBlocks.Tests.Fixtures;

// [Obsolete("Use SQL Server instead.")]
public class MongoDbFixture
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder().WithReplicaSet().Build();

    public string ConnectionString => $"{_mongoDbContainer.GetConnectionString()}?directConnection=true";

    public async Task DisposeAsync() => await _mongoDbContainer.StopAsync();

    public async Task InitializeAsync() => await _mongoDbContainer.StartAsync();
}