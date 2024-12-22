using Heyer.Modules.Hiring.Domain.Companies;

namespace Heyer.Modules.Hiring.Domain.JobOffers;

public record CompanyDetails
{
    public CompanyId CompanyId { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    private CompanyDetails()
    {
    }

    public CompanyDetails(CompanyId companyId, string name)
    {
        CompanyId = companyId;
        Name = name;
    }
}