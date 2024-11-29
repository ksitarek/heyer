using System.Net;
using FluentAssertions;
using Heyer.Storage.API.Endpoints.Store;
using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Tests.Utils;
using MongoDB.Driver;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

public class DeleteEndpointTests
{
    [Test]
    public async Task DownloadEndpoint_WithoutAuthorization_WillReturn401()
    {
        // Arrange
        await using var factory = ApplicationFactory.Create();
        var client = factory.CreateClient();
        
        // Act
        var response = await Delete(client, Guid.NewGuid().ToString());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Test]
    public async Task DownloadEndpoint_WithInvalidKey_WillReturnOk()
    {
        // Arrange
        await using var factory = ApplicationFactory.Create();
        var client = factory.CreateAuthorizedClient();
        
        // Act
        var response = await Delete(client, Guid.NewGuid().ToString());
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Test]
    public async Task DownloadEndpoint_WithValidKey_WillReturnOk()
    {
        // Arrange
        await using var factory = ApplicationFactory.Create();
        var client = factory.CreateAuthorizedClient();
        var storeResult = await Store(client, "IntegrationTests/Endpoints/test-file.png");
        
        // Act
        var response = await Delete(client, storeResult.FileHandle);
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var collection = factory.GetRequiredService<IMongoCollection<StorageRegistryEntry>>();
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, storeResult.FileHandle);
        var anyResult = await collection.Find(filter).AnyAsync();
        anyResult.Should().BeFalse();
    }
    
    private async Task<HttpResponseMessage> Delete(HttpClient client, string fileHandle)
    {
        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Delete;
        request.RequestUri = new Uri($"/delete/{fileHandle}", UriKind.Relative);
        
        var response = await client.SendAsync(request);
        
        return response;
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

        var storeResult = await response.ReadContentAs<StoreResult>();

        if (storeResult == null)
        {
            throw new ApplicationException("Failed to store test file.");
        }
        
        return storeResult;
    }
}