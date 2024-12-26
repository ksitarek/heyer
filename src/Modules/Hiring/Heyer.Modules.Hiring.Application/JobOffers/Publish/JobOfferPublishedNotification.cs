using Heyer.BuildingBlocks.Application.Notifications;
using Heyer.Modules.Hiring.Domain.JobOffers.Events;
using ExecutionContext = Heyer.BuildingBlocks.Application.Authorization.ExecutionContext;

namespace Heyer.Modules.Hiring.Application.JobOffers.Publish;

public class JobOfferPublishedNotification : IDomainEventNotification<JobOfferPublished>
{
    public JobOfferPublishedNotification(Guid id, JobOfferPublished domainEvent, ExecutionContext executionContext)
    {
        Id = id;
        DomainEvent = domainEvent;
        ExecutionContext = executionContext;
    }

    public JobOfferPublished DomainEvent { get; }
    public ExecutionContext ExecutionContext { get; }

    public Guid Id { get; init; }
}