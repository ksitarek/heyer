using Microsoft.EntityFrameworkCore;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

internal class DbContextInboxStore<T> : IInboxStore
    where T : DbContext, IInboxContext
{
    private readonly T _context;

    public DbContextInboxStore(T context) => _context = context;

    public Task<List<InboxMessage>> GetUnprocessedMessages() =>
        _context.InboxMessages.Where(x => x.ProcessedAt == null).ToListAsync();

    public async Task SetProcessedAt(Guid messageId, DateTime processedAt)
    {
        var message = await _context.InboxMessages.FirstAsync(x => x.Id == messageId);

        message.ProcessedAt = processedAt;

        await _context.SaveChangesAsync();
    }

    public Task Store(InboxMessage inboxMessage)
    {
        _context.InboxMessages.Add(inboxMessage);

        return _context.SaveChangesAsync();
    }
}