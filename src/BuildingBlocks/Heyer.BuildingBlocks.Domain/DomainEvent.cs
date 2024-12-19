using MediatR;

namespace Heyer.BuildingBlocks.Domain;

public abstract record DomainEvent : INotification
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }

    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}