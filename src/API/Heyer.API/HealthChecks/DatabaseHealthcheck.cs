using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace Heyer.API.HealthChecks;

public class DatabaseHealthcheck : IHealthCheck
{
    private readonly IMongoDatabase _database;

    public DatabaseHealthcheck(IMongoDatabase database) => _database = database;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                          CancellationToken cancellationToken = new())
    {
        var collections = await _database.ListCollectionsAsync(cancellationToken: cancellationToken);
        try
        {
            await collections.AnyAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy(e.Message, e);
        }
    }
}