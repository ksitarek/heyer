using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using Heyer.Storage.API.CleanupTempFiles;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Providers.Storage;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Heyer.Storage.API.Tests.UnitTests;

[Category("Unit")]
public class CleanupTempFilesTests
{
    private IRegistryStrategy _registryStrategy;
    private IStorageStrategy _storageStrategy;
    private CleanupTempFilesRequestHandler _handler;

    [SetUp]
    public void SetUp()
    {
        ConfigureRegistryStrategyMock();
        ConfigureStorageStrategyMock();

        _handler = new CleanupTempFilesRequestHandler(
            NullLogger<CleanupTempFilesRequestHandler>.Instance,
            _registryStrategy,
            _storageStrategy);
    }

    private void ConfigureRegistryStrategyMock()
    {
        _registryStrategy = Substitute.For<IRegistryStrategy>();
        _registryStrategy.DeleteAsync(
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(Result.Ok());
    }

    private void ConfigureStorageStrategyMock()
    {
        _storageStrategy = Substitute.For<IStorageStrategy>();
        _storageStrategy.DeleteAsync(
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(Result.Ok());
    }

    [Test]
    public async Task Handle_WhenNoExpiredFiles_ShouldReturnSuccess()
    {
        // Arrange
        _registryStrategy.GetExpiredTempFiles(Arg.Any<CancellationToken>())
            .Returns(Result.Ok(Enumerable.Empty<IFileProperties>()));

        // Act
        var result = await _handler.Handle(new CleanupTempFilesRequest(), CancellationToken.None);

        // Assert
        result.Should().BeSuccess();

        await _storageStrategy.DidNotReceive().DeleteAsync(
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());

        await _registryStrategy.DidNotReceive().DeleteAsync(
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WhenGetExpiredTempFilesFails_ShouldReturnError()
    {
        // Arrange
        _registryStrategy.GetExpiredTempFiles(Arg.Any<CancellationToken>())
            .Returns(Result.Fail("Reason"));

        // Act
        var result = await _handler.Handle(new CleanupTempFilesRequest(), CancellationToken.None);

        // Assert
        result.Should().BeFailure()
            .And.HaveError("Failed to retrieve expired temp files.")
            .And.HaveReason("Reason");

        await _storageStrategy.DidNotReceive().DeleteAsync(
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());

        await _registryStrategy.DidNotReceive().DeleteAsync(
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WhenUnableToDeleteOneOfFiles_ShouldReturnError()
    {
        // Arrange
        var files = new List<IFileProperties>()
        {
            new StorageRegistryEntry()
            {
                Key = "test-key1",
            },
            new StorageRegistryEntry()
            {
                Key = "test-key2",
            }
        };
        
        _registryStrategy.GetExpiredTempFiles(Arg.Any<CancellationToken>())
            .Returns(Result.Ok<IEnumerable<IFileProperties>>(files));
        
        _registryStrategy.DeleteAsync(
                "test-key2",
                Arg.Any<CancellationToken>())
            .Returns(Result.Fail("Reason"));
        
        // Act
        var result = await _handler.Handle(new CleanupTempFilesRequest(), CancellationToken.None);
        
        // Assert
        result.Should().BeFailure()
            .And.HaveError("Failed to delete temp file with key test-key2.")
            .That.BeOfType<Error>().Which.Reasons.Select(x => x.Message).Should().Contain("Reason");
    }

    [Test]
    public async Task Handle_WhenExpiredFilesPresent_ShouldCallStorageAndRegistry()
    {
        // Arrange
        var files = new List<IFileProperties>()
        {
            new StorageRegistryEntry()
            {
                Key = "test-key1",
            },
            new StorageRegistryEntry()
            {
                Key = "test-key2",
            }
        };

        _registryStrategy.GetExpiredTempFiles(Arg.Any<CancellationToken>())
            .Returns(Result.Ok<IEnumerable<IFileProperties>>(files));

        // Act
        var result = await _handler.Handle(new CleanupTempFilesRequest(), CancellationToken.None);

        // Assert
        result.Should().BeSuccess();

        foreach (var file in files)
        {
            await _storageStrategy.Received(1).DeleteAsync(
                file.Key,
                Arg.Any<CancellationToken>());

            await _registryStrategy.Received(1).DeleteAsync(
                file.Key,
                Arg.Any<CancellationToken>());
        }
    }
}