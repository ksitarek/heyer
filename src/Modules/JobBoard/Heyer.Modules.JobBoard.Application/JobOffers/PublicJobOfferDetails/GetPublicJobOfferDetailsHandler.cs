using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.Hiring.PublishedLanguage;
using Heyer.Modules.JobBoard.Application.Mapping;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using MediatR;

namespace Heyer.Modules.JobBoard.Application.JobOffers.PublicJobOfferDetails;

public class GetPublicJobOfferDetailsHandler : IRequestHandler<GetPublicJobOfferDetails, Result<PublishedJobOfferDetails>>
{
    private readonly IPublishedJobOffersRepository _publishedJobOffersRepository;

    public GetPublicJobOfferDetailsHandler(IPublishedJobOffersRepository publishedJobOffersRepository) =>
        _publishedJobOffersRepository = publishedJobOffersRepository;

    public async Task<Result<PublishedJobOfferDetails>> Handle(GetPublicJobOfferDetails request,
                                                      CancellationToken cancellationToken)
    {
        var publishedJobOffer = await _publishedJobOffersRepository.GetPublishedJobOfferById(
            new PublishedJobOfferId(request.Guid),
            cancellationToken);

        if (publishedJobOffer is null)
        {
            return new NotFoundError();
        }

        return publishedJobOffer.MapToJobOfferDetails();
    }
}