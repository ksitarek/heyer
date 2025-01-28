using FluentResults;
using Heyer.BuildingBlocks.Application.HttpLanguage;

namespace Heyer.Modules.Hiring.Domain.JobOffers;

public interface IJobOffersRepository
{
    Task<Result> AddAsync(JobOffer jobOffer, CancellationToken cancellationToken = default);

    Task<bool> CheckForConflicts(JobOffer subject,
                                 CancellationToken cancellationToken = default);

    Task<JobOffer?> GetJobOfferById(JobOfferId jobOfferId, CancellationToken cancellationToken = default);
    IQueryable<JobOffer> GetPageQuery(FilteredListRequest filteredListRequest);

    Task<long> GetTotalCount(FilteredListRequest filteredListRequest,
                             CancellationToken cancellationToken = default);
}