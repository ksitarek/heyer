using NUnit.Framework.Internal;
using Testcontainers.MongoDb;

namespace Heyer.Storage.API.Tests.IntegrationTests.Fixtures;

public class MongoDbFixture
{
    private static readonly Randomizer Randomizer = new();
    private readonly MongoDbContainer _mongoDbContainer;

    public string ConnectionString => _mongoDbContainer.GetConnectionString();

    public MongoDbFixture()
    {
        var port = Randomizer.Next(27100, 27200);
        var username = Randomizer.GetString(8);
        var password = Randomizer.GetString(8);

        _mongoDbContainer = new MongoDbBuilder()
            .WithImage("mongo:8")
            .WithUsername(username)
            .WithPassword(password)
            .WithPortBinding(port, 27017)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.StopAsync();
    }
}