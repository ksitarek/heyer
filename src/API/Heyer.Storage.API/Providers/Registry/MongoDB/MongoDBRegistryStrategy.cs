using Heyer.Storage.API.Validators;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Heyer.Storage.API.Providers.Registry.MongoDB;

public class MongoDBRegistryStrategy : IRegistryStrategy
{
    private readonly IMongoCollection<StorageRegistryEntry> _collection;

    public MongoDBRegistryStrategy(IMongoCollection<StorageRegistryEntry> collection)
    {
        _collection = collection;
    }
    
    public Task RegisterNewFileAsync(string key, IFormFile file, CancellationToken cancellationToken = default)
    {
        var entry = new StorageRegistryEntry
        {
            Key = key,
            FileName = Path.GetFileName(file.FileName),
            ContentType = file.GetFileFormat()?.ToString() ?? "UNKNOWN",
            Size = file.Length,
            CreatedAt = DateTimeOffset.UtcNow
        };

        return _collection.InsertOneAsync(entry, cancellationToken: cancellationToken);
    }

    public Task Preserve(string key, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, key);
        var update = Builders<StorageRegistryEntry>.Update.Set(x => x.Preserve, true);

        return _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }
}