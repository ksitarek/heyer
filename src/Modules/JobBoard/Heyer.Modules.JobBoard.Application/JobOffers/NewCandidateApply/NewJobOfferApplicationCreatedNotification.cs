using Heyer.BuildingBlocks.Application.Notifications;
using Heyer.Modules.JobBoard.Domain.JobOffers.Events;
using ExecutionContext = Heyer.BuildingBlocks.Application.Authorization.ExecutionContext;

namespace Heyer.Modules.JobBoard.Application.JobOffers.NewCandidateApply;

public class NewJobOfferApplicationCreatedNotification : IDomainEventNotification<NewJobOfferApplicationCreated>
{
    public NewJobOfferApplicationCreatedNotification(Guid id,
                                                     NewJobOfferApplicationCreated domainEvent,
                                                     ExecutionContext executionContext)
    {
        Id = id;
        DomainEvent = domainEvent;
        ExecutionContext = executionContext;
    }

    public NewJobOfferApplicationCreated DomainEvent { get; }

    public ExecutionContext ExecutionContext { get; }
    public Guid Id { get; init; }
}