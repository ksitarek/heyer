using System.Net;
using FluentAssertions;
using Heyer.Storage.API.Tests.Utils;
using RestEase;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

public class DownloadEndpointTests : IntegrationTestsBase
{
    [Test]
    public async Task DownloadEndpoint_WithoutAuthorization_WillReturn401()
    {
        // Arrange
        var client = AppFactory.CreateApiClient();

        // Act
        var action = async () => await client.Download(Guid.NewGuid().ToString());

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task DownloadEndpoint_WithInvalidKey_WillReturn404()
    {
        // Arrange
        var client = AppFactory.CreateAuthorizedApiClient();

        // Act
        var action = async () => await client.Download(Guid.NewGuid().ToString());

        // Assert
        (await action.Should().ThrowAsync<ApiException>()).And.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task DownloadEndpoint_WithValidKey_WillReturnCorrectFile()
    {
        // Arrange
        var client = AppFactory.CreateAuthorizedApiClient();
        var filePath = "IntegrationTests/Endpoints/test-file.png";
        var storeResult = await client.Store(filePath);

        // Act
        var fileResponse = await client.Download(storeResult.FileHandle);

        // Assert
        fileResponse.ResponseMessage.Content.Headers.ContentDisposition!.FileName.Should().Be("test-file.png");
        fileResponse.ResponseMessage.Content.Headers.ContentType!.MediaType.Should().Be("image/png");

        await using var downloadedFileStream = await fileResponse.ResponseMessage.Content.ReadAsStreamAsync();
        await using var testFile = File.OpenRead(filePath);
        using var fromDisk = new StreamContent(testFile);

        downloadedFileStream.Length.Should().Be(fromDisk.Headers.ContentLength);

        AreStreamsEqual(
            downloadedFileStream, 
            await fromDisk.ReadAsStreamAsync()).Should().BeTrue();
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