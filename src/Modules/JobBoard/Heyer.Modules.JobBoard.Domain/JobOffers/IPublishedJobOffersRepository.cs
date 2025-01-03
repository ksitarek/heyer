using FluentResults;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public interface IPublishedJobOffersRepository
{
    Task<Result> AddAsync(PublishedJobOffer publishedJobOffer, CancellationToken cancellationToken = default);

    Task<PublishedJobOffer?> GetJobOfferById(PublishedJobOfferId publishedJobOfferId,
                                             CancellationToken cancellationToken = default);

    Task<List<PublishedJobOffer>> GetPageAsync(long startIx, long cnt, CancellationToken cancellationToken);

    Task<PublishedJobOffer?> GetPublishedJobOfferById(PublishedJobOfferId publishedJobOfferId,
                                                      CancellationToken cancellationToken = default);
}