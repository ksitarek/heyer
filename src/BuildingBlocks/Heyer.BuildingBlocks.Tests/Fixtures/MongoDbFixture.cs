using NUnit.Framework.Internal;
using Testcontainers.MongoDb;

namespace Heyer.BuildingBlocks.Tests.Fixtures;

public class MongoDbFixture
{
    private static readonly Randomizer _randomizer = new();
    private readonly MongoDbContainer _mongoDbContainer;

    public MongoDbFixture()
    {
        var port = _randomizer.Next(27100, 27200);

        _mongoDbContainer = new MongoDbBuilder().WithReplicaSet().Build();
    }

    public string ConnectionString => $"{_mongoDbContainer.GetConnectionString()}?directConnection=true";

    public async Task DisposeAsync() => await _mongoDbContainer.StopAsync();

    public async Task InitializeAsync() => await _mongoDbContainer.StartAsync();
    // await _mongoDbContainer.ExecScriptAsync("rs.initiate();");
}