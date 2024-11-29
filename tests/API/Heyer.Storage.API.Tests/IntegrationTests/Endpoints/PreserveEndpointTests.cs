using System.Net;
using FluentAssertions;
using Heyer.Storage.API.Client;
using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Tests.Utils;
using MongoDB.Driver;
using RestEase;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

public class PreserveEndpointTests : IntegrationTestsBase
{
    [Test]
    public async Task PreserveEndpoint_WithValidKey_ReturnsOk()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();
        var storeResult = await client.Store("IntegrationTests/Endpoints/test-file.png");
        
        // Act
        await client.Preserve(storeResult.FileHandle);
        
        // Assert
        var collection = AppFactory.GetRequiredService<IMongoCollection<StorageRegistryEntry>>();
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, storeResult.FileHandle);
        var entry = await collection.Find(filter).FirstAsync();
        entry.Should().NotBeNull();
        entry.Preserve.Should().BeTrue();
    }
    
    [Test]
    public async Task PreserveEndpoint_WithInvalidKey_ReturnsNotFound()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();
        
        // Act
        var action = async () => await client.Preserve("invalid-key");
        
        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Test]
    public async Task PreserveEndpoint_WithAlreadyPreservedKey_ReturnsOk()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();
        var storeResult = await client.Store("IntegrationTests/Endpoints/test-file.png");
        
        // Act
        await client.Preserve(storeResult.FileHandle);
        
        // Assert
        var collection = AppFactory.GetRequiredService<IMongoCollection<StorageRegistryEntry>>();
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, storeResult.FileHandle);
        var entry = await collection.Find(filter).FirstAsync();
        entry.Should().NotBeNull();
        entry.Preserve.Should().BeTrue();
    }
}