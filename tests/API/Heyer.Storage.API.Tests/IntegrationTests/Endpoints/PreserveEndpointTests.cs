using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Heyer.Storage.API.Endpoints.Store;
using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Tests.Utils;
using MongoDB.Driver;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

public class PreserveEndpointTests
{
    [Test]
    public async Task PreserveEndpoint_WithValidKey_ReturnsOk()
    {
        // Arrange
        await using var factory = ApplicationFactory.Create();
        var client = factory.CreateClient();
        var storeResult = await Store(client, "IntegrationTests/Endpoints/test-file.png");
        
        // Act
        var response = await Preserve(client, storeResult.FileHandle);
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var collection = factory.GetRequiredService<IMongoCollection<StorageRegistryEntry>>();
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, storeResult.FileHandle);
        var entry = await collection.Find(filter).FirstAsync();
        entry.Should().NotBeNull();
        entry.Preserve.Should().BeTrue();
    }
    
    [Test]
    public async Task PreserveEndpoint_WithInvalidKey_ReturnsNotFound()
    {
        // Arrange
        await using var factory = ApplicationFactory.Create();
        var client = factory.CreateClient();
        
        // Act
        var response = await Preserve(client, Guid.NewGuid().ToString());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Test]
    public async Task PreserveEndpoint_WithAlreadyPreservedKey_ReturnsOk()
    {
        // Arrange
        await using var factory = ApplicationFactory.Create();
        var client = factory.CreateClient();
        var storeResult = await Store(client, "IntegrationTests/Endpoints/test-file.png");
        
        // Act
        var response = await Preserve(client, storeResult.FileHandle);
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var collection = factory.GetRequiredService<IMongoCollection<StorageRegistryEntry>>();
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, storeResult.FileHandle);
        var entry = await collection.Find(filter).FirstAsync();
        entry.Should().NotBeNull();
        entry.Preserve.Should().BeTrue();
    }

    private async Task<HttpResponseMessage> Preserve(HttpClient client, string fileKey)
    {
        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri($"/preserve/{fileKey}", UriKind.Relative);
        request.Headers.Add("RequestVerificationToken", await client.GetCsrfToken());
        // request.Content = JsonContent.Create(new {key = fileKey});
        return await client.SendAsync(request);
    }
    private async Task<StoreResult> Store(HttpClient client, string filePath)
    {
        await using var testFile = File.OpenRead(filePath);
        using var fileStream = new StreamContent(testFile);
        using var formData = new MultipartFormDataContent();

        formData.Add(fileStream, "file", testFile.Name);
        
        var csrfToken = await client.GetCsrfToken();

        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri("/store", UriKind.Relative);
        request.Headers.Add("RequestVerificationToken", csrfToken);
        request.Content = formData;

        var response = await client.SendAsync(request);

        var storeResult = await response.ReadContentAs<StoreResult>()!;

        if (storeResult == null)
        {
            throw new ApplicationException("Failed to store test file.");
        }
        
        return storeResult;
    }
}