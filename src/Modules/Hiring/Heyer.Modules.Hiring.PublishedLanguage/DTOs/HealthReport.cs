using System.Text.Json.Serialization;

namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record HealthReport
{
    [JsonConstructor]
    public HealthReport(HealthCheckStatus Status, IDictionary<string, HealthReportResult> Results)
    {
        this.Status = Status;
        this.Results = Results;
    }

    public HealthCheckStatus Status { get; init; }
    public IDictionary<string, HealthReportResult> Results { get; init; }

    public void Deconstruct(out HealthCheckStatus Status, out IDictionary<string, HealthReportResult> Results)
    {
        Status = this.Status;
        Results = this.Results;
    }
}