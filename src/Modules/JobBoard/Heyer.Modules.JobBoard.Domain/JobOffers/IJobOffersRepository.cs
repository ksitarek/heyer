using FluentResults;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public interface IJobOffersRepository
{
    Task<Result> AddAsync(JobOffer jobOffer, CancellationToken cancellationToken = default);
    Task<JobOffer?> GetJobOfferById(JobOfferId jobOfferId, CancellationToken cancellationToken = default);
}