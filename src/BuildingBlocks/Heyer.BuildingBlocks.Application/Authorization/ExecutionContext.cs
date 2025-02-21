using System.Text.Json.Serialization;

namespace Heyer.BuildingBlocks.Application.Authorization;

public record ExecutionContext
{
    [JsonConstructor]
    public ExecutionContext(Guid UserId, Guid CompanyId, string CompanyName)
    {
        this.UserId = UserId;
        this.CompanyId = CompanyId;
        this.CompanyName = CompanyName;
    }

    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; }

    public void Deconstruct(out Guid userId, out Guid companyId, out string companyName)
    {
        userId = UserId;
        companyId = CompanyId;
        companyName = CompanyName;
    }
}