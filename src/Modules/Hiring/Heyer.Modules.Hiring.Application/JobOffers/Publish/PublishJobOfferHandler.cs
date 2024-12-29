using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers.Publish;

public class PublishJobOfferHandler : ICommandHandler<PublishJobOffer>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public PublishJobOfferHandler(IJobOffersRepository jobOffersRepository) =>
        _jobOffersRepository = jobOffersRepository;

    public async Task<Result> Handle(PublishJobOffer request, CancellationToken cancellationToken)
    {
        var jobOffer = await _jobOffersRepository.GetJobOfferById(
            new JobOfferId(request.JobOfferId),
            cancellationToken);

        if (jobOffer is null)
        {
            return new NotFoundError();
        }

        var publishResult = jobOffer.Publish(request.PublishUntil);

        return publishResult;
    }
}