using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using IHealthCheck = Heyer.BuildingBlocks.Infrastructure.HealthChecks.IHealthCheck;

namespace Heyer.Modules.JobBoard.Infrastructure.HealthChecks;

internal class JobBoardDatabaseHealthcheck : IHealthCheck
{
    public string Name => "JobBoardDatabase";
    public TimeSpan Timeout => TimeSpan.FromSeconds(3);

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                          CancellationToken cancellationToken = new())
    {
        try
        {
            using var scope = JobBoardModuleCompositionRoot.CreateScope();

            await using var dbContext = scope.ServiceProvider.GetService<JobBoardContext>()!;

            await dbContext.PublishedJobOffers.AnyAsync(cancellationToken);

            return HealthCheckResult.Healthy();
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy(e.Message, e);
        }
    }
}