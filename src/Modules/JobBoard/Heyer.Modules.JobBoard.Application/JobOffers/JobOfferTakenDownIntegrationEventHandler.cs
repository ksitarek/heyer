using FluentResults;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.PublishedLanguage.IntegrationEvents;
using Heyer.Modules.JobBoard.Domain.JobOffers;

namespace Heyer.Modules.JobBoard.Application.JobOffers;

public class JobOfferTakenDownIntegrationEventHandler : IEventHandler<JobOfferTakenDownIntegrationEvent>
{
    private readonly IPublishedJobOffersRepository _publishedJobOffersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public JobOfferTakenDownIntegrationEventHandler(IPublishedJobOffersRepository publishedJobOffersRepository,
                                                    IUnitOfWork unitOfWork)
    {
        _publishedJobOffersRepository = publishedJobOffersRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(JobOfferTakenDownIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var publishedJobOfferId = new PublishedJobOfferId(notification.JobOfferId);

        var result = await _publishedJobOffersRepository.DeleteAsync(publishedJobOfferId, cancellationToken);

        if (result.IsFailed && result.Errors[0] is ExceptionalError exceptionalError)
        {
            throw new Exception("Failed to take down job offer.", exceptionalError.Exception);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}