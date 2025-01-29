using Heyer.BuildingBlocks.Application.Notifications;
using Heyer.Modules.Hiring.Domain.JobOffers.Events;
using ExecutionContext = Heyer.BuildingBlocks.Application.Authorization.ExecutionContext;

namespace Heyer.Modules.Hiring.Application.JobOffers.TakeDown;

public class JobOfferTakenDownNotification : IDomainEventNotification<JobOfferTakenDown>
{
    public JobOfferTakenDownNotification(Guid id, JobOfferTakenDown domainEvent, ExecutionContext executionContext)
    {
        Id = id;
        DomainEvent = domainEvent;
        ExecutionContext = executionContext;
    }

    public JobOfferTakenDown DomainEvent { get; }

    public ExecutionContext ExecutionContext { get; }

    public Guid Id { get; init; }
}