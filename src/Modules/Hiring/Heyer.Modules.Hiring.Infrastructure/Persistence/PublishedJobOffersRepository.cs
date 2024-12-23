using FluentResults;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.Hiring.Infrastructure.Persistence;

internal class JobOffersRepository : IJobOffersRepository
{
    private readonly HiringDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JobOffersRepository(HiringDbContext context, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> AddAsync(JobOffer publishedJobOffer,
                                       CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.JobOffers.AddAsync(publishedJobOffer, cancellationToken);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(new Error("Could not add job offer").CausedBy(e));
        }
    }

    public Task<JobOffer?> GetJobOfferById(JobOfferId publishedJobOfferId,
                                           CancellationToken cancellationToken = default) =>
        _context.JobOffers
            .Where(x => x.Id == publishedJobOfferId)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<JobOffer?> GetPublishedJobOfferById(JobOfferId publishedJobOfferId,
                                                    CancellationToken cancellationToken = default)
    {
        var now = _dateTimeProvider.UtcNow();

        return _context.JobOffers
            .Where(x => x.Id == publishedJobOfferId)
            .Where(x => x.PublishedUntil == null || x.PublishedUntil >= now)
            .FirstOrDefaultAsync(cancellationToken);
    }
}