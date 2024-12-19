using Heyer.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

internal class DomainEventsAccessor : IDomainEventsAccessor
{
    private readonly DbContext _context;

    public DomainEventsAccessor(DbContext context) => _context = context;

    private IEnumerable<Entity> LocalDomainEntitiesWithEvents => _context.ChangeTracker
        .Entries<Entity>()
        .Select(x => x.Entity)
        .Where(x => x.DomainEvents.Any());

    public void ClearAllDomainEvents()
    {
        foreach (var entity in LocalDomainEntitiesWithEvents)
        {
            entity.ClearDomainEvents();
        }
    }

    public IReadOnlyCollection<DomainEvent> GetAllDomainEvents() =>
        LocalDomainEntitiesWithEvents
            .SelectMany(x => x.DomainEvents)
            .ToList();
}