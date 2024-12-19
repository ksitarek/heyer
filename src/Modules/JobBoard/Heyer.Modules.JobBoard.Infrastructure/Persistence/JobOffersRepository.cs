using FluentResults;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence;

internal class JobOffersRepository : IJobOffersRepository
{
    private readonly JobBoardContext _context;

    public JobOffersRepository(JobBoardContext context) => _context = context;

    public async Task<Result> AddAsync(JobOffer jobOffer, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.JobOffers.AddAsync(jobOffer, cancellationToken);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(new Error("Could not add job offer").CausedBy(e));
        }
    }

    public Task<JobOffer?> GetJobOfferById(JobOfferId jobOfferId, CancellationToken cancellationToken = default) =>
        _context.JobOffers
            .Where(x => x.Id == jobOfferId)
            .FirstOrDefaultAsync(cancellationToken);
}