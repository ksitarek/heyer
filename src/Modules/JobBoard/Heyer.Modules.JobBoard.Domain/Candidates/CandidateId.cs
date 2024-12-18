namespace Heyer.Modules.JobBoard.Domain.Candidates;

public record CandidateId(Guid Guid)
{
    public static CandidateId CreateNew() => new(Guid.NewGuid());
}