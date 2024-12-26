namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

public interface IOutboxStore
{
    Task<List<OutboxMessage>> GetUnprocessedMessages();

    Task SetProcessedAt(Guid messageId, DateTime processedAt);
    Task Store(OutboxMessage outboxMessage);
}