using Heyer.BuildingBlocks.Infrastructure.Integration;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Domain.JobOffers.Events;
using Heyer.Modules.Hiring.PublishedLanguage.IntegrationEvents;

namespace Heyer.Modules.Hiring.Application.JobOffers.TakeDown;

public class
    JobOfferTakenDownNotificationHandler : IDomainNotificationHandler<JobOfferTakenDownNotification, JobOfferTakenDown>
{
    private readonly IEventBus _eventBus;
    private readonly IJobOffersRepository _jobOfferRepository;

    public JobOfferTakenDownNotificationHandler(IEventBus eventBus,
                                                IJobOffersRepository jobOfferRepository)
    {
        _eventBus = eventBus;
        _jobOfferRepository = jobOfferRepository;
    }

    public async Task Handle(JobOfferTakenDownNotification notification, CancellationToken cancellationToken)
    {
        var jobOffer =
            await _jobOfferRepository.GetJobOfferById(notification.DomainEvent.JobOfferId, cancellationToken);

        if (jobOffer == null)
        {
            // We should never step into this handler if the job offer does not exist
            throw new InvalidOperationException($"Job offer with id {notification.DomainEvent.JobOfferId} not found");
        }

        var integrationEvent = new JobOfferTakenDownIntegrationEvent(
            Guid.NewGuid(),
            notification.DomainEvent.OccurredOn,
            notification.DomainEvent.JobOfferId.Guid);

        await _eventBus.Publish(integrationEvent);
    }
}