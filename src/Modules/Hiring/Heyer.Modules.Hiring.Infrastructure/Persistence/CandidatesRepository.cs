using FluentResults;
using Heyer.Modules.Hiring.Domain.Candidates;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.Hiring.Infrastructure.Persistence;

internal class CandidatesRepository : ICandidatesRepository
{
    private readonly HiringDbContext _context;

    public CandidatesRepository(HiringDbContext context) => _context = context;

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

    public Task<Candidate?> GetCandidateById(CandidateId candidateId, CancellationToken cancellationToken = default) =>
        _context.Candidates
            .Where(x => x.Id == candidateId)
            .FirstOrDefaultAsync(cancellationToken);
}