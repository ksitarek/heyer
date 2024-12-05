using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.Storage.API.Validators;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Heyer.Storage.API.Providers.Registry.MongoDB;

public class MongoDBRegistryStrategy : IRegistryStrategy
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOptions<RegistryStrategyOptions> _options;
    private readonly IMongoCollection<StorageRegistryEntry> _collection;
    private readonly ILogger<MongoDBRegistryStrategy> _logger;

    public MongoDBRegistryStrategy(
        IDateTimeProvider dateTimeProvider,
        IOptions<RegistryStrategyOptions> options,
        IMongoCollection<StorageRegistryEntry> collection,
        ILogger<MongoDBRegistryStrategy> logger)
    {
        _dateTimeProvider = dateTimeProvider;
        _options = options;
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
            CreatedAt = _dateTimeProvider.UtcNow()
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
        try
        {
            var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, key);
            var entry = await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

            return entry == null
                ? Result.Fail(new NotFoundError())
                : entry;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result> DeleteAsync(string key, CancellationToken cancellationToken)
    {
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, key);
        try
        {
            await _collection.DeleteOneAsync(filter, cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return new Error("Failed to delete storage registry entry.").CausedBy(ex);
        }
    }

    public async Task<Result<IEnumerable<IFileProperties>>> GetExpiredTempFiles(CancellationToken cancellationToken)
    {
        var refDate = _dateTimeProvider.UtcNow().AddSeconds(- _options.Value.TempFileLifespan);
        
        var filter = Builders<StorageRegistryEntry>.Filter.And(
            Builders<StorageRegistryEntry>.Filter.Lte(x => x.CreatedAt, refDate), 
            Builders<StorageRegistryEntry>.Filter.Eq(x => x.Preserve, false));

        return await _collection.Find(filter)
            .ToListAsync(cancellationToken);
    }
}