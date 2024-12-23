namespace Heyer.Modules.Hiring.PublishedLanguage;

public record HealthReport(HealthCheckStatus Status, IDictionary<string, HealthReportResult> Results);