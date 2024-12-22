using FluentResults;
using Heyer.API.Client.PublishedLanguage;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.JobBoard.Application.Mapping;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using MediatR;

namespace Heyer.Modules.JobBoard.Application.JobOffers.PublicJobOfferDetails;

public class GetPublicJobOfferDetailsHandler : IRequestHandler<GetPublicJobOfferDetails, Result<JobOfferDetails>>
{
    private readonly IPublishedJobOffersRepository _publishedJobOffersRepository;

    public GetPublicJobOfferDetailsHandler(IPublishedJobOffersRepository publishedJobOffersRepository) =>
        _publishedJobOffersRepository = publishedJobOffersRepository;

    public async Task<Result<JobOfferDetails>> Handle(GetPublicJobOfferDetails request,
                                                      CancellationToken cancellationToken)
    {
        var jobOffer = await _publishedJobOffersRepository.GetPublishedJobOfferById(
            new PublishedJobOfferId(request.Guid),
            cancellationToken);

        if (jobOffer is null)
        {
            return new NotFoundError();
        }

        return jobOffer.MapToJobOfferDetails();
    }
}