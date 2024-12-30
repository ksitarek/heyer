using Microsoft.EntityFrameworkCore;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

public interface IInboxStore
{
    Task<List<InboxMessage>> GetUnprocessedMessages();

    Task SetProcessedAt(Guid messageId, DateTime processedAt);
    Task Store(InboxMessage inboxMessage);
}

public interface IInboxContext
{
    DbSet<InboxMessage> InboxMessages { get; init; }
}

public interface IOutboxContext
{
    DbSet<OutboxMessage> OutboxMessages { get; init; }
}