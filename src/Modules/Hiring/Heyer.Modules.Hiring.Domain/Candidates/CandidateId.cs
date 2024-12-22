namespace Heyer.Modules.Hiring.Domain.Candidates;

public record CandidateId
{
    public Guid Guid { get; private set; }

    private CandidateId()
    {
    }

    public CandidateId(Guid guid) => Guid = guid;

    public static CandidateId CreateNew() => new(Guid.NewGuid());
}