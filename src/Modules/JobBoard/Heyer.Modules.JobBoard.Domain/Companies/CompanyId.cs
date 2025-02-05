namespace Heyer.Modules.JobBoard.Domain.Companies;

public record CompanyId
{
    public Guid Id { get; private set; }

    private CompanyId()
    {
    }

    public CompanyId(Guid id) => Id = id;

    public static CompanyId CreateNew() => new(Guid.CreateVersion7());
}