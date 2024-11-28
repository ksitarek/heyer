using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Heyer.Storage.API.Endpoints.Store;
using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Tests.Utils;
using MongoDB.Driver;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

public class DownloadEndpointTests
{
    [Test]
    public async Task DownloadEndpoint_WithoutAuthorization_WillReturn401()
    {
        // Arrange
        await using var factory = ApplicationFactory.Create();
        var client = factory.CreateClient();
        
        // Act
        var response = await Download(client, Guid.NewGuid().ToString());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Test]
    public async Task DownloadEndpoint_WithInvalidKey_WillReturn404()
    {
        // Arrange
        await using var factory = ApplicationFactory.Create();
        var client = factory.CreateAuthorizedClient();
        
        // Act
        var response = await Download(client, Guid.NewGuid().ToString());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Test]
    public async Task DownloadEndpoint_WithValidKey_WillReturnCorrectFile()
    {
        // Arrange
        await using var factory = ApplicationFactory.Create();
        var client = factory.CreateAuthorizedClient();
        var filePath = "IntegrationTests/Endpoints/test-file.png";
        var storeResult = await Store(client, filePath);
        
        // Act
        var response = await Download(client, storeResult.FileHandle);
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        response.Content.Headers.ContentDisposition!.FileName.Should().Be("test-file.png");
        response.Content.Headers.ContentType!.MediaType.Should().Be("image/png");
        
        await using var testFile = File.OpenRead(filePath);
        using var fromDisk = new StreamContent(testFile);
        
        AreStreamsEqual(
            await response.Content.ReadAsStreamAsync(), 
            await fromDisk.ReadAsStreamAsync()).Should().BeTrue();
    }

    private async Task<HttpResponseMessage> Download(HttpClient client, string fileKey)
    {
        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Get;
        request.RequestUri = new Uri($"/download/{fileKey}", UriKind.Relative);
        request.Headers.Add("RequestVerificationToken", await client.GetCsrfToken());
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

        var storeResult = await response.ReadContentAs<StoreResult>();

        if (storeResult == null)
        {
            throw new ApplicationException("Failed to store test file.");
        }
        
        return storeResult;
    }
    
    private bool AreStreamsEqual(Stream stream, Stream other)
    {
        const int bufferSize = 2048;
        if (other.Length != stream.Length)
        {
            return false;
        }

        byte[] buffer = new byte[bufferSize];
        byte[] otherBuffer = new byte[bufferSize];
        while ((_ = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            var _ = other.Read(otherBuffer, 0, otherBuffer.Length);

            if (!otherBuffer.SequenceEqual(buffer))
            {
                stream.Seek(0, SeekOrigin.Begin);
                other.Seek(0, SeekOrigin.Begin);
                return false;
            }
        }
        stream.Seek(0, SeekOrigin.Begin);
        other.Seek(0, SeekOrigin.Begin);
        return true;
    }
}