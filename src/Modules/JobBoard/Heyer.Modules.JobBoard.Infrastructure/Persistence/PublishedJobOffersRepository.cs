using FluentResults;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence;

internal class PublishedJobOffersRepository : IPublishedJobOffersRepository
{
    private readonly JobBoardContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PublishedJobOffersRepository(JobBoardContext context, IDateTimeProvider dateTimeProvider)
    {
        _context = context;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> AddAsync(PublishedJobOffer publishedJobOffer,
                                       CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.PublishedJobOffers.AddAsync(publishedJobOffer, cancellationToken);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(new Error("Could not add job offer").CausedBy(e));
        }
    }

    public Task<PublishedJobOffer?> GetJobOfferById(PublishedJobOfferId publishedJobOfferId,
                                                    CancellationToken cancellationToken = default) =>
        _context.PublishedJobOffers
            .Where(x => x.Id == publishedJobOfferId)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<PublishedJobOffer?> GetPublishedJobOfferById(PublishedJobOfferId publishedJobOfferId,
                                                             CancellationToken cancellationToken = default)
    {
        var now = _dateTimeProvider.UtcNow();

        return _context.PublishedJobOffers
            .Where(x => x.Id == publishedJobOfferId)
            .Where(x => x.PublishedUntil == null || x.PublishedUntil >= now)
            .FirstOrDefaultAsync(cancellationToken);
    }
}