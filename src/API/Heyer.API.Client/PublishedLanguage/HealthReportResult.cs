namespace Heyer.API.Client.PublishedLanguage;

public record HealthReportResult(HealthCheckStatus Status, string Description, Dictionary<string, object> Data);