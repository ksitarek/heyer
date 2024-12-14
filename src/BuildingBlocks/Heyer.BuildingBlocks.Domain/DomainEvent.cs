using MediatR;

namespace Heyer.BuildingBlocks.Domain;

public abstract record DomainEvent : INotification {
    public Guid Id { get; }
    public DateTime OccurredOn { get; }

    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}