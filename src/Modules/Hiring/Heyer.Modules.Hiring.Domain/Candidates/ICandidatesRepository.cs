using FluentResults;

namespace Heyer.Modules.Hiring.Domain.Candidates;

public interface ICandidatesRepository
{
    Task<Result> AddCandidate(Candidate candidate, CancellationToken cancellationToken = default);
    Task<Candidate?> GetCandidateById(CandidateId candidateId, CancellationToken cancellationToken = default);
}