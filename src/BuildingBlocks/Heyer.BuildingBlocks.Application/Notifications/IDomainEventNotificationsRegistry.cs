using Heyer.BuildingBlocks.Domain;

namespace Heyer.BuildingBlocks.Application.Notifications;

public interface IDomainEventNotificationsRegistry
{
    void Add<TNotification, TDomainEvent>()
        where TNotification : IDomainEventNotification<TDomainEvent>
        where TDomainEvent : DomainEvent;

    bool Contains(string name);
    Type GetNotificationType(string name);
}