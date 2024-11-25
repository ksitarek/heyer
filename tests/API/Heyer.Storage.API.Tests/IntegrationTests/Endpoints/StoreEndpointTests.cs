using System.Net;
using FluentAssertions;
using Heyer.Storage.API.Endpoints.Store;
using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Tests.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

public class StoreEndpointTests
{
    private ApplicationFactory _factory;
    private IMongoCollection<StorageRegistryEntry> _collection;


    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _factory = new ApplicationFactory();
        await _factory.InitializeDependenciesAsync();

        _collection = (_factory.Services.GetRequiredService(typeof(IMongoCollection<StorageRegistryEntry>))
            as IMongoCollection<StorageRegistryEntry>)!;
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        // cleanup test files
        var storePath = ApplicationFactory.InMemoryConfiguration[Config.StorageStrategy_FilesystemStorage_RootPath];
        if (Directory.Exists(storePath!))
            Directory.Delete(storePath, true);

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
        content!.FileHandle.Should().NotBeNull();

        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, content.FileHandle);
        var entry = await _collection.Find(filter).FirstAsync();
        
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
        var client = _factory.CreateClient();

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
}