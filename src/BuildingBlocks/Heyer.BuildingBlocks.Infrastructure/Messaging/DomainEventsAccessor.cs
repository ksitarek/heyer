using Heyer.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

internal class DomainEventsAccessor : IDomainEventsAccessor
{
    private readonly DbContext _context;
    
    private IEnumerable<Entity> LocalDomainEntitiesWithEvents => _context.ChangeTracker
        .Entries<Entity>()
        .Select(x => x.Entity)
        .Where(x => x.DomainEvents.Any());

    public DomainEventsAccessor(DbContext context)
    {
        _context = context;
    }
    
    public IReadOnlyCollection<DomainEvent> GetAllDomainEvents()
    {
        return LocalDomainEntitiesWithEvents
            .SelectMany(x => x.DomainEvents)
            .ToList();
    }

    public void ClearAllDomainEvents()
    {
        foreach (var entity in LocalDomainEntitiesWithEvents)
        {
            entity.ClearDomainEvents();
        }
    }
}