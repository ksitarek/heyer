using MongoDB.Driver;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

internal class MongoDbOutboxStore : IOutboxStore
{
    private readonly IMongoCollection<OutboxMessage> _collection;

    public MongoDbOutboxStore(IMongoDatabase mongoDatabase) =>
        _collection = mongoDatabase.GetCollection<OutboxMessage>("OutboxMessages");

    public Task<List<OutboxMessage>> GetUnprocessedMessages() =>
        _collection.Find(x => x.ProcessedAt == null).ToListAsync();

    public async Task SetProcessedAt(Guid messageId, DateTime processedAt) =>
        await _collection.UpdateOneAsync(
            Builders<OutboxMessage>.Filter.Eq(x => x.Id, messageId),
            Builders<OutboxMessage>.Update.Set(x => x.ProcessedAt, processedAt));

    public Task Store(OutboxMessage outboxMessage) => _collection.InsertOneAsync(outboxMessage);
}