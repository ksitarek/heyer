using FluentResults;
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
    
    public async Task<Result<int>> CommitAsync(CancellationToken cancellationToken)
    {
        var result = await _domainEventDispatcher.DispatchDomainEventsAsync(cancellationToken);
        if (result.IsFailed)
        {
            return Result.Fail<int>(result.Errors);
        }

        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return Result.Fail<int>(new Error("An error occurred while saving changes to the database.").CausedBy(e));
        }
    }
}