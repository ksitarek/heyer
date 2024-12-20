namespace Heyer.API.Client.PublishedLanguage;

public enum RemoteWork
{
    Unknown,
    No,
    Hybrid,
    Yes
}

public record HealthReport(HealthCheckStatus Status, IDictionary<string, HealthReportResult> Results);

public enum HealthCheckStatus
{
    Unknown,
    Healthy,
    Degraded,
    Unhealthy
}

public record HealthReportResult(HealthCheckStatus Status, string Description, Dictionary<string, object> Data);