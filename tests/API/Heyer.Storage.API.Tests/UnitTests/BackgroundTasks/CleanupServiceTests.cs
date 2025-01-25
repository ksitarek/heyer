using FluentResults;
using Heyer.Storage.API.BackgroundTasks;
using Heyer.Storage.API.CleanupTempFiles;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;

namespace Heyer.Storage.API.Tests.UnitTests.BackgroundTasks;

[Category("Unit")]
public class CleanupServiceTests
{
    private IMediator _mediatorMock = null!;
    private CleanupServiceOptions _options = null!;
    private CleanupService _service = null!;

    [SetUp]
    public Task SetUp()
    {
        _options = new CleanupServiceOptions { Interval = 1 };

        _mediatorMock = Substitute.For<IMediator>();

        _service = new CleanupService(
            NullLogger<CleanupService>.Instance,
            Options.Create(_options),
            _mediatorMock);

        return Task.CompletedTask;
    }

    [Test]
    [Parallelizable(ParallelScope.None)]
    public async Task ShouldHandleAllExceptions()
    {
        // Arrange
        _mediatorMock.Send(Arg.Any<CleanupTempFilesRequest>(), Arg.Any<CancellationToken>())
            .Throws(new Exception("Test exception"));

        // Act
        var action = async () =>
        {
            await _service.StartAsync(CancellationToken.None);
            await Task.Delay(TimeSpan.FromSeconds(_options.Interval * 2));
        };

        // Assert
        await action.ShouldNotThrowAsync();
    }

    [Test]
    [Parallelizable(ParallelScope.None)]
    public async Task StartAsync_ShouldStartTimerAndTriggerCleanup()
    {
        // Arrange
        _mediatorMock.Send(Arg.Any<CleanupTempFilesRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result.Ok());

        // Act
        await _service.StartAsync(CancellationToken.None);
        await Task.Delay(TimeSpan.FromSeconds(_options.Interval * 4));

        // Assert
        await _mediatorMock
            .Received(5).Send(Arg.Any<CleanupTempFilesRequest>(), Arg.Any<CancellationToken>());
    }

    [Test]
    [Parallelizable(ParallelScope.None)]
    public async Task StopAsync_ShouldStopTimer()
    {
        // Arrange
        _mediatorMock.Send(Arg.Any<CleanupTempFilesRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result.Ok());

        // Act
        await _service.StartAsync(CancellationToken.None);
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        await _service.StopAsync(CancellationToken.None);
        await Task.Delay(TimeSpan.FromSeconds(_options.Interval * 2));

        // Assert
        await _mediatorMock
            .Received(1).Send(Arg.Any<CleanupTempFilesRequest>(), Arg.Any<CancellationToken>());
    }

    [TearDown]
    public async Task TearDown() => await _service.DisposeAsync();
}