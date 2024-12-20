using Heyer.Storage.API.Providers.Storage;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Heyer.Storage.API.HealthChecks;

public class StorageHealthcheck : IHealthCheck
{
    private readonly IStorageStrategy _storageStrategy;

    public StorageHealthcheck(IStorageStrategy storageStrategy) => _storageStrategy = storageStrategy;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                          CancellationToken cancellationToken = new())
    {
        var result = await _storageStrategy.GetAvailableFreeSpaceAsync(cancellationToken);
        if (!result.IsSuccess)
        {
            return HealthCheckResult.Unhealthy("Failed to check available free space.");
        }

        var health = result.Value switch
        {
            < 100 * 1024 * 1024 => HealthStatus.Unhealthy,
            < 1000 * 1024 * 1024 => HealthStatus.Degraded,
            _ => HealthStatus.Healthy
        };

        return new HealthCheckResult(health);
    }
}