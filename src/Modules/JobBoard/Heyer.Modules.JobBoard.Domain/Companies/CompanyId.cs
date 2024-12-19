namespace Heyer.Modules.JobBoard.Domain.Companies;

public record CompanyId(Guid Id)
{
    public static CompanyId CreateNew() => new(Guid.NewGuid());
}