using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Heyer.BuildingBlocks.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public UnitOfWork(DbContext context, IDomainEventDispatcher domainEventDispatcher)
    {
        _context = context;
        _domainEventDispatcher = domainEventDispatcher;
    }
    
    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        await _domainEventDispatcher.DispatchEventsAsync(cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken);
    }
}