using FluentResults;
using Heyer.BuildingBlocks.Tests.Extensions;
using Heyer.Storage.API.Client.PublishedLanguage;
using Heyer.Storage.API.Download;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Providers.Storage;
using NSubstitute;

namespace Heyer.Storage.API.Tests.UnitTests.Download;

[Category("Unit")]
public class DownloadRequestHandlerTests
{
    private DownloadRequestHandler _handler = null!;
    private IRegistryStrategy _registryStrategy = null!;
    private IStorageStrategy _storageStrategy = null!;

    [Test]
    public async Task DownloadRequest_WhenBothStrategiesSucceed_ShouldReturnOkResult()
    {
        // Arrange
        var storageRegistryEntry = new StorageRegistryEntry
        {
            Key = "key",
            FileName = "test-file.png",
            ContentType = "image/png"
        };

        await using var fileStream = File.OpenRead("Utils/TestFiles/test-file.png");

        var request = new DownloadRequest("key");
        _storageStrategy.GetAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Ok<Stream>(fileStream));
        _registryStrategy.GetAsync("key", Arg.Any<CancellationToken>())
            .Returns(Result.Ok<IFileProperties>(storageRegistryEntry));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.ShouldBeSuccess(new DownloadResponse("test-file.png", "image/png", fileStream));
    }

    [Test]
    public async Task DownloadRequestHandler_WhenRegistryStrategyFails_ShouldReturnFailedResult()
    {
        // Arrange
        var request = new DownloadRequest("key");
        _storageStrategy.GetAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Ok());
        _registryStrategy.GetAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Fail("error"));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.ShouldBeFailure("error");
    }

    [Test]
    public async Task DownloadRequestHandler_WhenStorageStrategyFails_ShouldReturnFailedResult()
    {
        // Arrange
        var request = new DownloadRequest("key");
        _storageStrategy.GetAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Fail("error"));
        _registryStrategy.GetAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Ok());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.ShouldBeFailure("error");
    }

    [SetUp]
    public void Setup()
    {
        _storageStrategy = Substitute.For<IStorageStrategy>();
        _registryStrategy = Substitute.For<IRegistryStrategy>();

        _handler = new DownloadRequestHandler(_registryStrategy, _storageStrategy);
    }
}