namespace Heyer.BuildingBlocks.Infrastructure.HealthChecks;

public interface IHealthCheck : Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck
{
    public string Name { get; }
    public TimeSpan Timeout { get; }
}