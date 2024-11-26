using FluentResults;
using Heyer.Storage.API.Validators;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Heyer.Storage.API.Providers.Registry.MongoDB;

public class MongoDBRegistryStrategy : IRegistryStrategy
{
    private readonly IMongoCollection<StorageRegistryEntry> _collection;
    private readonly ILogger<MongoDBRegistryStrategy> _logger;

    public MongoDBRegistryStrategy(
        IMongoCollection<StorageRegistryEntry> collection,
        ILogger<MongoDBRegistryStrategy> logger)
    {
        _collection = collection;
        _logger = logger;
    }

    public async Task<Result> RegisterNewFileAsync(
        string key,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        var entry = new StorageRegistryEntry
        {
            Key = key,
            FileName = Path.GetFileName(file.FileName),
            ContentType = file.GetFileFormat()?.ToString() ?? "UNKNOWN",
            Size = file.Length,
            CreatedAt = DateTimeOffset.UtcNow
        };

        try
        {
            await _collection.InsertOneAsync(entry, cancellationToken: cancellationToken);
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to register new file.");
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result> SetPreserveAsync(
        string key,
        bool preserve,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, key);
        var update = Builders<StorageRegistryEntry>.Update.Set(x => x.Preserve, preserve);

        try
        {
            await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to set preserve flag.");
            return Result.Fail(e.Message);
        }
    }
}