using FluentResults;
using Heyer.API.Client.PublishedLanguage;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.JobBoard.Application.Mapping;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using MediatR;

namespace Heyer.Modules.JobBoard.Application.JobOffers.PublicJobOfferDetails;

public class GetPublicJobOfferDetailsHandler : IRequestHandler<GetPublicJobOfferDetails, Result<JobOfferDetails>>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public GetPublicJobOfferDetailsHandler(IJobOffersRepository jobOffersRepository) =>
        _jobOffersRepository = jobOffersRepository;

    public async Task<Result<JobOfferDetails>> Handle(GetPublicJobOfferDetails request,
                                                      CancellationToken cancellationToken)
    {
        var jobOffer = await _jobOffersRepository.GetPublishedJobOfferById(
            new JobOfferId(request.Guid),
            cancellationToken);

        if (jobOffer is null)
        {
            return new NotFoundError();
        }

        return jobOffer.MapToJobOfferDetails();
    }
}