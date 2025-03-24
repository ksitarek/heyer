using MediatR;
using ExecutionContext = Heyer.BuildingBlocks.Application.Authorization.ExecutionContext;

namespace Heyer.BuildingBlocks.Infrastructure.Integration;

public abstract record IntegrationEvent : INotification
{
    public IntegrationEvent(Guid id, DateTime occurredOn)
    {
        Id = id;
        OccurredOn = occurredOn;
    }

    public Guid Id { get; }

    public DateTime OccurredOn { get; }
}

public abstract record IntegrationEventWithContext : IntegrationEvent
{
    public IntegrationEventWithContext(Guid id, DateTime occurredOn, ExecutionContext executionContext) :
        base(id, occurredOn) =>
        ExecutionContext = executionContext;

    public ExecutionContext ExecutionContext { get; }
}