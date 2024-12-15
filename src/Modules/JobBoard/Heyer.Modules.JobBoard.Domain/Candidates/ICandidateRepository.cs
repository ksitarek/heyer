namespace Heyer.Modules.JobBoard.Domain.Candidates;

public interface ICandidateRepository
{
    Task AddAsync(Candidate candidate, CancellationToken cancellationToken = default);
    Task<Candidate?> GetByIdAsync(CandidateId candidateId, CancellationToken cancellationToken = default);
}