using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Storage;
using Heyer.Storage.API.Store;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Heyer.Storage.API.Tests.UnitTests.Store;

[Category("Unit")]
public class StoreRequestHandlerTests
{
    private StoreRequestHandler _handler;
    private IRegistryStrategy _registryStrategy;
    private IStorageStrategy _storageStrategy;

    [SetUp]
    public void SetUp()
    {
        _storageStrategy = Substitute.For<IStorageStrategy>();
        _registryStrategy = Substitute.For<IRegistryStrategy>();

        var storageStrategyResult = Result.Ok();
        var registryStrategyResult = Result.Ok();

        _storageStrategy
            .StoreAsync(Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(storageStrategyResult);

        _registryStrategy
            .RegisterNewFileAsync(Arg.Any<string>(), Arg.Any<IFormFile>(), Arg.Any<CancellationToken>())
            .Returns(registryStrategyResult);

        _handler = new StoreRequestHandler(_storageStrategy, _registryStrategy);
    }

    [Test]
    public async Task ShouldReturnErrorWhenRegistryStrategyFails()
    {
        // Arrange
        var request = new StoreRequest(Substitute.For<IFormFile>());
        _registryStrategy
            .RegisterNewFileAsync(Arg.Any<string>(), Arg.Any<IFormFile>(), Arg.Any<CancellationToken>())
            .Returns(Result.Fail("Registry strategy failed"));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeFailure()
            .Which.HasError(e => e.Message == "Registry strategy failed");

        await _storageStrategy
            .Received(1)
            .StoreAsync(
                Arg.Any<string>(),
                Arg.Any<Stream>(),
                Arg.Any<CancellationToken>());

        await _registryStrategy
            .Received(1)
            .RegisterNewFileAsync(
                Arg.Any<string>(),
                request.File,
                Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldReturnErrorWhenStorageStrategyFails()
    {
        // Arrange
        var request = new StoreRequest(Substitute.For<IFormFile>());
        _storageStrategy
            .StoreAsync(Arg.Any<string>(), Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(Result.Fail("Storage strategy failed"));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeFailure()
            .Which.HasError(e => e.Message == "Storage strategy failed");

        await _storageStrategy
            .Received(1)
            .StoreAsync(
                Arg.Any<string>(),
                Arg.Any<Stream>(),
                Arg.Any<CancellationToken>());

        await _registryStrategy
            .DidNotReceive()
            .RegisterNewFileAsync(
                Arg.Any<string>(),
                request.File,
                Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ShouldStoreFileAndRegisterNewFile()
    {
        // Arrange
        var request = new StoreRequest(Substitute.For<IFormFile>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeSuccess();

        await _storageStrategy
            .Received(1)
            .StoreAsync(
                Arg.Any<string>(),
                Arg.Any<Stream>(),
                Arg.Any<CancellationToken>());

        await _registryStrategy
            .Received(1)
            .RegisterNewFileAsync(
                Arg.Any<string>(),
                request.File,
                Arg.Any<CancellationToken>());
    }
}