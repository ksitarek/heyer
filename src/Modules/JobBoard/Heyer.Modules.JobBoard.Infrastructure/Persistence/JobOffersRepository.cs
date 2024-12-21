using FluentResults;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence;

internal class JobOffersRepository : IJobOffersRepository
{
    private readonly JobBoardContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JobOffersRepository(JobBoardContext context, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }

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

    public Task<JobOffer?> GetPublishedJobOfferById(JobOfferId jobOfferId,
                                                    CancellationToken cancellationToken = default)
    {
        var now = _dateTimeProvider.UtcNow();

        return _context.JobOffers
            .Where(x => x.Id == jobOfferId)
            .Where(x => x.PublishedAt != null && x.PublishedAt <= now)
            .Where(x => x.PublishedUntil == null || x.PublishedUntil >= now)
            .FirstOrDefaultAsync(cancellationToken);
    }
}