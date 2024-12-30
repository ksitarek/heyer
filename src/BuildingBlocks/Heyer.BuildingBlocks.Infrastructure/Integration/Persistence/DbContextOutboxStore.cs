using Microsoft.EntityFrameworkCore;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

internal class DbContextOutboxStore<T> : IOutboxStore
    where T : DbContext, IOutboxContext
{
    private readonly T _dbContext;

    public DbContextOutboxStore(T dbContext) => _dbContext = dbContext;

    public Task<List<OutboxMessage>> GetUnprocessedMessages() =>
        _dbContext.OutboxMessages.Where(x => x.ProcessedAt == null).ToListAsync();

    public Task SetProcessedAt(Guid messageId, DateTime processedAt)
    {
        var message = _dbContext.OutboxMessages.First(x => x.Id == messageId);

        message.ProcessedAt = processedAt;

        return _dbContext.SaveChangesAsync();
    }

    public async Task Store(OutboxMessage outboxMessage)
    {
        await _dbContext.OutboxMessages.AddAsync(outboxMessage);

        await _dbContext.SaveChangesAsync();
    }
}