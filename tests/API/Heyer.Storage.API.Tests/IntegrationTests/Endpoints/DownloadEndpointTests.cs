using System.Net;
using Heyer.Storage.API.Tests.Utils;
using RestEase;
using Shouldly;

namespace Heyer.Storage.API.Tests.IntegrationTests.Endpoints;

[Category("Integration")]
public class DownloadEndpointTests : StorageApiIntegrationTestsBase
{
    [Test]
    public async Task DownloadEndpoint_WithInvalidKey_WillReturn404()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(Guid.CreateVersion7());

        // Act
        var action = async () => await client.Download(Guid.CreateVersion7().ToString());

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task DownloadEndpoint_WithoutAuthorization_WillReturn401()
    {
        // Arrange
        var client = _appFactory.CreateApiClient();

        // Act
        var action = async () => await client.Download(Guid.CreateVersion7().ToString());

        // Assert
        var exception = await action.ShouldThrowAsync<ApiException>();
        exception.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task DownloadEndpoint_WithValidKey_WillReturnCorrectFile()
    {
        // Arrange
        var client = _appFactory.CreateAuthorizedApiClient(Guid.CreateVersion7());
        var filePath = "Utils/TestFiles/test-file.png";
        var storeResult = await client.Store(filePath);

        // Act
        var fileResponse = await client.Download(storeResult.FileHandle);

        // Assert
        fileResponse.ResponseMessage.Content.Headers.ContentDisposition!.FileName.ShouldBe("test-file.png");
        fileResponse.ResponseMessage.Content.Headers.ContentType!.MediaType.ShouldBe("image/png");

        await using var downloadedFileStream = await fileResponse.ResponseMessage.Content.ReadAsStreamAsync();
        await using var testFile = File.OpenRead(filePath);
        using var fromDisk = new StreamContent(testFile);

        downloadedFileStream.Length.ShouldBe(fromDisk.Headers.ContentLength!.Value);

        AreStreamsEqual(
            downloadedFileStream,
            await fromDisk.ReadAsStreamAsync()).ShouldBeTrue();
    }

    private bool AreStreamsEqual(Stream stream, Stream other)
    {
        const int bufferSize = 2048;
        if (other.Length != stream.Length)
        {
            return false;
        }

        var buffer = new byte[bufferSize];
        var otherBuffer = new byte[bufferSize];
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