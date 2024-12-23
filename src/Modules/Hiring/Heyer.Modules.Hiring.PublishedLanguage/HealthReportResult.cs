namespace Heyer.Modules.Hiring.PublishedLanguage;

public record HealthReportResult(HealthCheckStatus Status, string Description, Dictionary<string, object> Data);