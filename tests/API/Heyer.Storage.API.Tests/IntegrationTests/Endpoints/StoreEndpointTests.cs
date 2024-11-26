using System.Net;
using FluentAssertions;
using Heyer.Storage.API.Endpoints.Store;
using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Tests.Utils;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

public class StoreEndpointTests
{
    [Test]
    public async Task StoreEndpoint_WithValidFile_ReturnsOkWithFileHandle()
    {
        // Arrange
        await using var factory = CreateFactory("MongoDB", "Filesystem");
        var client = factory.CreateClient();

        // Act
        var response = await Store(client, "IntegrationTests/Endpoints/test-file.png");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = await response.ReadContentAs<StoreResult>();
        content.Should().NotBeNull();
        content!.FileHandle.Should().NotBeNull();

        var collection = factory.GetRequiredService<IMongoCollection<StorageRegistryEntry>>();
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, content.FileHandle);
        var entry = await collection.Find(filter).FirstAsync();
        entry.Should().NotBeNull();
        entry.Key.Should().Be(content.FileHandle);
        entry.FileName.Should().Be("test-file.png");
        entry.ContentType.Should().Be("image/png");
        entry.Size.Should().Be(7936);
    }

    [Test]
    public async Task StoreEndpoint_WithInvalidFile_ReturnsOkWithFileHandle()
    {
        // Arrange
        await using var factory = CreateFactory("MongoDB", "Filesystem");
        var client = factory.CreateClient();

        // Act
        var response = await Store(client, "IntegrationTests/Endpoints/test-file.docx");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var validationDetails = (await response.ReadContentAs<ValidationProblemDetails>())!;
        validationDetails.Should().NotBeNull();
        validationDetails.Errors.Should().HaveCount(2).And.ContainKeys("File.FileName", "File");
        validationDetails.Errors["File"].Should().Contain("Invalid file format.");
        validationDetails.Errors["File.FileName"].Should().Contain("Invalid file extension.");
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
    
    private ApplicationFactory CreateFactory(string registryStrategyType, string storageStrategyType)
    {
        return new(new()
        {
            [Config.RegistryStrategy_Type] = registryStrategyType,
            [Config.StorageStrategy_Type] = storageStrategyType,
            [Config.StorageStrategy_FilesystemStorage_RootPath] = "IntegrationTests/Endpoints/StoreEndpointTests",
        });
    }
}