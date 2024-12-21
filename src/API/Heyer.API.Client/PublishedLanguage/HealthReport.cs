namespace Heyer.API.Client.PublishedLanguage;

public record HealthReport(HealthCheckStatus Status, IDictionary<string, HealthReportResult> Results);