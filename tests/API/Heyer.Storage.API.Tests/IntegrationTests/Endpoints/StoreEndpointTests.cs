using System.Text;
using FluentAssertions;
using Heyer.Storage.API.Endpoints.Store;
using Heyer.Storage.API.Tests.Utils;
using Microsoft.AspNetCore.Http;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

public class StoreEndpointTests
{
    private ApplicationFactory _factory;

    [SetUp]
    public Task Setup()
    {
        _factory = new ApplicationFactory();
        return Task.CompletedTask;
    }
    
    [TearDown]
    public void TearDown()
    {
        _factory.Dispose();
    }
    
    [Test]
    public async Task StoreEndpoint_WithValidFile_ReturnsOkWithFileHandle()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Act
        var response = await Store(client, "IntegrationTests/Endpoints/test-file.png");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.ReadContentAs<StoreResult>();
        content.Should().NotBeNull();
        
        Guid.TryParse(content!.FileHandle, out var _).Should().BeTrue();
    }

    private async Task<HttpResponseMessage> Store(HttpClient client, string filePath)
    {
        await using var testFile = File.OpenRead(filePath);
        using var fileStream = new StreamContent(testFile);
        using var formData = new MultipartFormDataContent();
        
        formData.Add(fileStream, "file", testFile.Name);

        var request = new HttpRequestMessage();
        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri("/store", UriKind.Relative);
        request.Headers.Add("RequestVerificationToken", await client.GetCsrfToken());
        request.Content = formData;

        return await client.SendAsync(request);
    }
}