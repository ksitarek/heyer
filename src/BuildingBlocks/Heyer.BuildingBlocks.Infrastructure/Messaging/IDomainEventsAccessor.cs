using Heyer.BuildingBlocks.Domain;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public interface IDomainEventsAccessor
{
    IReadOnlyCollection<DomainEvent> GetAllDomainEvents();
    
    void ClearAllDomainEvents();
}