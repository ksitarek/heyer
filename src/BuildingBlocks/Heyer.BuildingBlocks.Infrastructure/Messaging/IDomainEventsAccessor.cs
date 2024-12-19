using Heyer.BuildingBlocks.Domain;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public interface IDomainEventsAccessor
{
    void ClearAllDomainEvents();
    IReadOnlyCollection<DomainEvent> GetAllDomainEvents();
}