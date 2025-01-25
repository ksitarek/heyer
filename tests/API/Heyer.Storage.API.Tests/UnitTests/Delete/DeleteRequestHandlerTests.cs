using FluentResults;
using Heyer.BuildingBlocks.Tests.Extensions;
using Heyer.Storage.API.Client.PublishedLanguage;
using Heyer.Storage.API.Delete;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Storage;
using NSubstitute;

namespace Heyer.Storage.API.Tests.UnitTests.Delete;

[Category("Unit")]
public class DeleteRequestHandlerTests
{
    private DeleteRequestHandler _handler = null!;
    private IRegistryStrategy _registryStrategy = null!;
    private IStorageStrategy _storageStrategy = null!;

    [Test]
    public async Task DeleteRequestHandler_WhenBothStrategiesSucceed_ShouldReturnOkResult()
    {
        // Arrange
        var request = new DeleteRequest("key");
        _storageStrategy.DeleteAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Ok());
        _registryStrategy.DeleteAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Ok());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.ShouldBeSuccess();
    }

    [Test]
    public async Task DeleteRequestHandler_WhenRegistryStrategyFails_ShouldReturnFailedResult()
    {
        // Arrange
        var request = new DeleteRequest("key");
        _storageStrategy.DeleteAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Ok());
        _registryStrategy.DeleteAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Fail("error"));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.ShouldBeFailure("error");
    }

    [Test]
    public async Task DeleteRequestHandler_WhenStorageStrategyFails_ShouldReturnFailedResult()
    {
        // Arrange
        var request = new DeleteRequest("key");
        _storageStrategy.DeleteAsync("key", Arg.Any<CancellationToken>()).Returns(Result.Fail("error"));

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

        _handler = new DeleteRequestHandler(_registryStrategy, _storageStrategy);
    }
}