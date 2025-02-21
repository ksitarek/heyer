using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Storage.API.Client;

namespace Heyer.Modules.JobBoard.Application.JobOffers.NewCandidateApply;

public class NewCandidateApplyToJobOfferHandler : ICommandHandler<NewCandidateApplyToJobOffer>
{
    private readonly IPublishedJobOffersRepository _publishedJobOffersRepository;
    private readonly IStorageApiClient _storageApiClient;

    public NewCandidateApplyToJobOfferHandler(IPublishedJobOffersRepository publishedJobOffersRepository,
                                              IStorageApiClient storageApiClient)
    {
        _publishedJobOffersRepository = publishedJobOffersRepository;
        _storageApiClient = storageApiClient;
    }

    public async Task<Result> Handle(NewCandidateApplyToJobOffer request, CancellationToken cancellationToken)
    {
        var publishedJobOffer =
            await _publishedJobOffersRepository.GetJobOfferById(request.PublishedJobOfferId, cancellationToken);

        if (publishedJobOffer is null)
        {
            return new NotFoundError();
        }

        try
        {
            await _storageApiClient.Preserve(request.ResumeKey);
        }
        catch (Exception e)
        {
            return Result.Fail(new ExceptionalError("Failed to preserve resume", e));
        }

        return publishedJobOffer.AddApplication(new JobOfferApplication(
                                                    request.FirstName,
                                                    request.LastName,
                                                    request.Email,
                                                    request.ResumeKey,
                                                    request.IncludeInCandidatePool,
                                                    request.Attributes));
    }
}