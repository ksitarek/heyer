using Heyer.BuildingBlocks.Domain;
using MediatR;
using ExecutionContext = Heyer.BuildingBlocks.Application.Authorization.ExecutionContext;

namespace Heyer.BuildingBlocks.Application.Notifications;

public interface IDomainEventNotification : INotification
{
    public ExecutionContext ExecutionContext { get; }
    public Guid Id { get; init; }
}

public interface IDomainEventNotification<out TDomainEvent> : IDomainEventNotification
    where TDomainEvent : DomainEvent
{
    public TDomainEvent DomainEvent { get; }
}