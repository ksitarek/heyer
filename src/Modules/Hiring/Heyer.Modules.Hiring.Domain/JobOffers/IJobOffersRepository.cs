using FluentResults;

namespace Heyer.Modules.Hiring.Domain.JobOffers;

public interface IJobOffersRepository
{
    Task<Result> AddAsync(JobOffer jobOffer, CancellationToken cancellationToken = default);
    Task<JobOffer?> GetJobOfferById(JobOfferId jobOfferId, CancellationToken cancellationToken = default);
    IQueryable<JobOffer> GetPageQuery();
}