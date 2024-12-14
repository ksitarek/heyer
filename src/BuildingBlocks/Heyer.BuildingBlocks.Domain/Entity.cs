using System.Collections.ObjectModel;

namespace Heyer.BuildingBlocks.Domain;

public abstract class Entity
{
    private List<DomainEvent>? _domainEvents;

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly() ?? ReadOnlyCollection<DomainEvent>.Empty;

    protected void AddDomainEvent(DomainEvent @event)
    {
        _domainEvents ??= new();
        _domainEvents.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}