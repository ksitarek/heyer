namespace Heyer.Modules.Candidates.Domain.Candidates;

public record CandidateId(Guid Guid)
{
    public static CandidateId CreateNew()
    {
        return new CandidateId(Guid.NewGuid());
    }
}