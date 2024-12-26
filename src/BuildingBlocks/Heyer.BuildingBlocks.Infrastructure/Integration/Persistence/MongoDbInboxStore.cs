using MongoDB.Driver;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

internal class MongoDbInboxStore : IInboxStore
{
    private readonly IMongoCollection<InboxMessage> _collection;

    public MongoDbInboxStore(IMongoDatabase mongoDatabase) =>
        _collection = mongoDatabase.GetCollection<InboxMessage>("InboxMessages");

    public Task<List<InboxMessage>> GetUnprocessedMessages() =>
        _collection.Find(x => x.ProcessedAt == null).ToListAsync();

    public async Task SetProcessedAt(Guid messageId, DateTime processedAt) =>
        await _collection.UpdateOneAsync(
            Builders<InboxMessage>.Filter.Eq(x => x.Id, messageId),
            Builders<InboxMessage>.Update.Set(x => x.ProcessedAt, processedAt));

    public Task Store(InboxMessage inboxMessage) => _collection.InsertOneAsync(inboxMessage);
}