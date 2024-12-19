using NUnit.Framework.Internal;
using Testcontainers.MongoDb;

namespace Heyer.BuildingBlocks.Tests.Fixtures;

public class MongoDbFixture
{
    private static readonly Randomizer Randomizer = new();
    private readonly MongoDbContainer _mongoDbContainer;

    public MongoDbFixture()
    {
        var port = Randomizer.Next(27100, 27200);
        var username = Randomizer.GetString(8);
        var password = Randomizer.GetString(8);

        _mongoDbContainer = new MongoDbBuilder()
            .WithImage("mongo:8")
            .WithUsername(username)
            .WithPortBinding(port, 27017)
            .Build();
    }

    public string ConnectionString => _mongoDbContainer.GetConnectionString();

    public async Task DisposeAsync() => await _mongoDbContainer.StopAsync();

    public async Task InitializeAsync() => await _mongoDbContainer.StartAsync();
}