using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using Heyer.Storage.API.Preserve;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Storage;
using NSubstitute;

namespace Heyer.Storage.API.Tests.UnitTests.Preserve;

[Category("Unit")]
public class PreserveRequestHandlerTests
{
    private PreserveRequestHandler _handler;
    private IRegistryStrategy _registryStrategy;
    private IStorageStrategy _storageStrategy;

    [Test]
    public async Task DownloadRequest_WhenBothStrategiesSucceed_ShouldReturnOkResult()
    {
        // Arrange
        var request = new PreserveRequest("key");
        _storageStrategy.PreserveAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Ok());
        _registryStrategy.SetPreserveAsync("key", true, Arg.Any<CancellationToken>()).Returns(Result.Ok());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeSuccess();

        await _storageStrategy.Received(1).PreserveAsync("key", Arg.Any<CancellationToken>());
        await _registryStrategy.Received(1).SetPreserveAsync("key", true, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task DownloadRequestHandler_WhenRegistryStrategyFails_ShouldReturnFailedResult()
    {
        // Arrange
        var request = new PreserveRequest("key");
        _storageStrategy.PreserveAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Ok());
        _registryStrategy.SetPreserveAsync("key", true, Arg.Any<CancellationToken>()).Returns(Result.Fail("error"));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeFailure().And.HaveError("error");
    }

    [Test]
    public async Task DownloadRequestHandler_WhenStorageStrategyFails_ShouldReturnFailedResult()
    {
        // Arrange
        var request = new PreserveRequest("key");
        _storageStrategy.PreserveAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Fail("error"));
        _registryStrategy.SetPreserveAsync("key", true, Arg.Any<CancellationToken>()).Returns(Result.Ok());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeFailure().And.HaveError("error");
    }

    [SetUp]
    public void Setup()
    {
        _storageStrategy = Substitute.For<IStorageStrategy>();
        _registryStrategy = Substitute.For<IRegistryStrategy>();

        _handler = new PreserveRequestHandler(_storageStrategy, _registryStrategy);
    }
}