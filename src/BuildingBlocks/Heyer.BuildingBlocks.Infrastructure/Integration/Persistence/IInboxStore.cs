namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

public interface IInboxStore
{
    Task<List<InboxMessage>> GetUnprocessedMessages();

    Task SetProcessedAt(Guid messageId, DateTime processedAt);
    Task Store(InboxMessage inboxMessage);
}