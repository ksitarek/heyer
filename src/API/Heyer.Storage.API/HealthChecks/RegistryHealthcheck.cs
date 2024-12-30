using Heyer.Storage.API.Providers.Registry;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Heyer.Storage.API.HealthChecks;

public class RegistryHealthcheck : IHealthCheck
{
    private readonly IRegistryStrategy _registryStrategy;

    public RegistryHealthcheck(IRegistryStrategy registryStrategy) =>
        _registryStrategy = registryStrategy;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                          CancellationToken cancellationToken = new())
    {
        try
        {
            await _registryStrategy.GetAsync("doesn't matter", cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy(e.Message, e);
        }
    }
}