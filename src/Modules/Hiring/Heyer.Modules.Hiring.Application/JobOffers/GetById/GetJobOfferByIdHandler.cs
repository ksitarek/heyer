using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.Hiring.Application.Mapping;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage;
using MediatR;

namespace Heyer.Modules.Hiring.Application.JobOffers.GetById;

public class GetJobOfferByIdHandler : IRequestHandler<GetJobOfferById, Result<JobOfferDetails>>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public GetJobOfferByIdHandler(IJobOffersRepository jobOffersRepository) =>
        _jobOffersRepository = jobOffersRepository;

    public async Task<Result<JobOfferDetails>> Handle(GetJobOfferById request, CancellationToken cancellationToken)
    {
        var jobOffer = await _jobOffersRepository.GetJobOfferById(new JobOfferId(request.Guid), cancellationToken);

        if (jobOffer is null)
        {
            return new NotFoundError();
        }

        return jobOffer.MapToJobOfferDetails();
    }
}