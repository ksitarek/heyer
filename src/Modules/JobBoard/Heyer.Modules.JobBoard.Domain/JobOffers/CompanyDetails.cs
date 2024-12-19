using Heyer.Modules.JobBoard.Domain.Companies;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

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