using System.Net;
using System.Text.Json;
using FluentAssertions;
using Heyer.Storage.API.Client;
using Heyer.Storage.API.Endpoints.Store;
using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Tests.Utils;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RestEase;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

public class StoreEndpointTests : IntegrationTestsBase
{
    [Test]
    public async Task StoreEndpoint_WithValidFile_ReturnsOkWithFileHandle()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();

        // Act
        var storeResult = await client.Store("IntegrationTests/Endpoints/test-file.png");

        // Assert
        storeResult.Should().NotBeNull();
        storeResult.FileHandle.Should().NotBeNull();
        
        // TODO Validate file exists

        var collection = AppFactory.GetRequiredService<IMongoCollection<StorageRegistryEntry>>();
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, storeResult.FileHandle);
        var entry = await collection.Find(filter).FirstAsync();
        entry.Should().NotBeNull();
        entry.Key.Should().Be(storeResult.FileHandle);
        entry.FileName.Should().Be("test-file.png");
        entry.ContentType.Should().Be("image/png");
        entry.Size.Should().Be(2620);
    }

    [Test]
    public async Task StoreEndpoint_WithInvalidFile_ReturnsOkWithFileHandle()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();

        // Act
        var action = async () => await client.Store("IntegrationTests/Endpoints/test-file.docx");

        // Assert
        var exception = await action.Should().ThrowAsync<ApiException>()
            .Where(e => e.StatusCode == HttpStatusCode.BadRequest);

        var validationDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(exception.Which.Content!)!;
        
        validationDetails.Should().NotBeNull();
        validationDetails.Errors.Should().HaveCount(2).And.ContainKeys("File.FileName", "File");
        validationDetails.Errors["File"].Should().Contain("Invalid file format.");
        validationDetails.Errors["File.FileName"].Should().Contain("Invalid file extension.");
    }
}