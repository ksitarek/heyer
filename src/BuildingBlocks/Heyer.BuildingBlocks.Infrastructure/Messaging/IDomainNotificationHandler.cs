using Heyer.BuildingBlocks.Application.Notifications;
using Heyer.BuildingBlocks.Domain;
using MediatR;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public interface
    IDomainNotificationHandler<in TDomainEventNotification, TDomainEvent> : INotificationHandler<
    TDomainEventNotification>
    where TDomainEventNotification : IDomainEventNotification<TDomainEvent>
    where TDomainEvent : DomainEvent
{
}