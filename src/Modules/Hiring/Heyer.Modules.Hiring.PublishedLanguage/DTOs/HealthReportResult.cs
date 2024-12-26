using System.Text.Json.Serialization;

namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record HealthReportResult
{
    [JsonConstructor]
    public HealthReportResult(HealthCheckStatus Status, string? Description, Dictionary<string, object> Data)
    {
        this.Status = Status;
        this.Description = Description;
        this.Data = Data;
    }

    public HealthCheckStatus Status { get; init; }
    public string? Description { get; init; }
    public Dictionary<string, object> Data { get; init; }

    public void Deconstruct(out HealthCheckStatus Status, out string? Description, out Dictionary<string, object> Data)
    {
        Status = this.Status;
        Description = this.Description;
        Data = this.Data;
    }
}