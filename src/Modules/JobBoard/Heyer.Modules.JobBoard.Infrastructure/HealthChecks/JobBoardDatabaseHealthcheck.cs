using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using IHealthCheck = Heyer.BuildingBlocks.Infrastructure.HealthChecks.IHealthCheck;

namespace Heyer.Modules.JobBoard.Infrastructure.HealthChecks;

public class JobBoardDatabaseHealthcheck : IHealthCheck
{
    private readonly IMongoDatabase _database;

    public JobBoardDatabaseHealthcheck(IMongoDatabase database) => _database = database;

    public string Name => "JobBoardDatabase";
    public TimeSpan Timeout => TimeSpan.FromSeconds(3);

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