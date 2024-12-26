using System.Text.Json.Serialization;

namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record CompanyDetails
{
    public Guid CompanyId { get; private set; }
    public string Name { get; private set; } = null!;

    private CompanyDetails()
    {
    }

    [JsonConstructor]
    public CompanyDetails(Guid companyId, string name)
    {
        CompanyId = companyId;
        Name = name;
    }
}