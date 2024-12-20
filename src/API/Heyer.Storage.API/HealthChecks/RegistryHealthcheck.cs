using Heyer.Storage.API.Providers.Registry.MongoDB;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace Heyer.Storage.API.HealthChecks;

public class RegistryHealthcheck : IHealthCheck
{
    private readonly IMongoCollection<StorageRegistryEntry> _mongoRegistryCollection;

    public RegistryHealthcheck(IMongoCollection<StorageRegistryEntry> mongoRegistryCollection) =>
        _mongoRegistryCollection = mongoRegistryCollection;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                          CancellationToken cancellationToken = new())
    {
        try
        {
            var cnt = await _mongoRegistryCollection.CountDocumentsAsync(FilterDefinition<StorageRegistryEntry>.Empty,
                                                                         cancellationToken: cancellationToken);

            return HealthCheckResult.Healthy(cnt.ToString());
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy(e.Message, e);
        }
    }
}