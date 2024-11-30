using Heyer.Storage.API.CleanupTempFiles;
using MediatR;

namespace Heyer.Storage.API;

public class CleanupService : IHostedService, IAsyncDisposable
{
    private readonly ILogger<CleanupService> _logger;
    private readonly IMediator _mediator;
    private Timer? _timer = null;

    public CleanupService(ILogger<CleanupService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cleanup Service is starting.");
        _timer = new Timer(Cleanup, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private async void Cleanup(object? state)
    {
        _logger.LogTrace("Cleanup Service is working.");

        var cleanupResult = await _mediator.Send(new CleanupTempFilesRequest(), CancellationToken.None);

        _logger.Log(
            cleanupResult.IsSuccess ? LogLevel.Information : LogLevel.Error,
            "Deleted {sCnt} temp files. {fCnt} files failed to delete.",
            cleanupResult.Successes.Count,
            cleanupResult.Errors.Count);
    }

    public async ValueTask DisposeAsync()
    {
        if (_timer != null) await _timer.DisposeAsync();
    }
}