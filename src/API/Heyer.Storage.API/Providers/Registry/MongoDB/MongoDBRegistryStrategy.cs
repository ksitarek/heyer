using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Storage.API.Validators;
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
            var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            return result switch
            {
                { MatchedCount: 1 } => Result.Ok(),
                { MatchedCount: 0 } => Result.Fail(new NotFoundError()),
                _ => Result.Fail("Unknown error.")
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to set preserve flag.");
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result> ValidateKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, key);
        return Result.OkIf(await _collection.Find(filter).AnyAsync(cancellationToken), "Key not found.");
    }

    public async Task<Result<IFileProperties>> GetAsync(string key, CancellationToken cancellationToken)
    {
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, key);
        var entry = await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

        return entry == null
            ? Result.Fail(new NotFoundError())
            : entry;
    }

    public async Task<Result> DeleteAsync(string key, CancellationToken cancellationToken)
    {
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, key);
        try
        {
            var r = await _collection.DeleteOneAsync(filter, cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return new Error("Failed to delete storage registry entry.").CausedBy(ex);
        }
    }
}