using FluentResults;
using Heyer.Modules.JobBoard.Domain.Candidates;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence;

internal class CandidatesRepository : ICandidatesRepository
{
    private readonly JobBoardContext _context;

    public CandidatesRepository(JobBoardContext context)
    {
        _context = context;
    }
    
    public Task<Candidate?> GetCandidateById(CandidateId candidateId, CancellationToken cancellationToken = default)
    {
        return _context.Candidates
            .Where(x => x.Id == candidateId)
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task<Result> AddCandidate(Candidate candidate, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Candidates.AddAsync(candidate, cancellationToken);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(new Error("Failed to add candidate").CausedBy(e));
        }
    }
}