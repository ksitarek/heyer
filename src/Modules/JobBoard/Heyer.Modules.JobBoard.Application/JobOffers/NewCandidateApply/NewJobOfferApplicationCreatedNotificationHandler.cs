using Heyer.BuildingBlocks.Infrastructure.Integration;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.PublishedLanguage.IntegrationEvents;
using Heyer.Modules.JobBoard.Domain.JobOffers.Events;
using ExecutionContext = Heyer.BuildingBlocks.Application.Authorization.ExecutionContext;

namespace Heyer.Modules.JobBoard.Application.JobOffers.NewCandidateApply;

public class NewJobOfferApplicationCreatedNotificationHandler : IDomainNotificationHandler<
    NewJobOfferApplicationCreatedNotification, NewJobOfferApplicationCreated>
{
    private readonly IEventBus _eventBus;

    public NewJobOfferApplicationCreatedNotificationHandler(IEventBus eventBus) => _eventBus = eventBus;

    public async Task Handle(NewJobOfferApplicationCreatedNotification notification,
                             CancellationToken cancellationToken)
    {
        var integrationEvent = new NewJobOfferApplicationCreatedIntegrationEventWithContext(
            Guid.NewGuid(),
            notification.DomainEvent.OccurredOn,
            new ExecutionContext(Guid.Empty,
                                 notification.DomainEvent.CompanyDetails.CompanyId,
                                 notification.DomainEvent.CompanyDetails.Name),
            notification.DomainEvent.PublishedJobOfferId.Guid,
            notification.DomainEvent.JobOfferApplication.FirstName,
            notification.DomainEvent.JobOfferApplication.LastName,
            notification.DomainEvent.JobOfferApplication.Email,
            notification.DomainEvent.JobOfferApplication.ResumeKey,
            notification.DomainEvent.JobOfferApplication.IncludeInCandidatePool,
            notification.DomainEvent.JobOfferApplication.Attributes);

        await _eventBus.Publish(integrationEvent);
    }
}