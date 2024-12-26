using MediatR;

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