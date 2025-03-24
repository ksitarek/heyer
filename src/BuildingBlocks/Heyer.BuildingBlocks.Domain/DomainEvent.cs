using MediatR;

namespace Heyer.BuildingBlocks.Domain;

public abstract record DomainEvent : INotification
{
    public Guid EventId { get; protected set; }
    public DateTime OccurredOn { get; protected set; }

    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}