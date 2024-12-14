using Heyer.Modules.Candidates.Domain.Candidates;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.Candidates.IntegrationEvents.Persistence.Candidates;


internal class CandidateRepository : ICandidateRepository
{
    private readonly CandidatesContext _context;

    public CandidateRepository(CandidatesContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Candidate candidate, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(candidate, cancellationToken);
    }

    public Task<Candidate?> GetByIdAsync(CandidateId candidateId, CancellationToken cancellationToken = default)
    {
        return _context.Candidates.FirstOrDefaultAsync(c => c.Id == candidateId, cancellationToken);
    }
}