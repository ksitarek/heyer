using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.Storage.API.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Heyer.Storage.API.Providers.Registry.Npgsql;

internal class NpgsqlRegistryStrategy : IRegistryStrategy
{
    private readonly StorageDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<NpgsqlRegistryStrategy> _logger;
    private readonly IOptions<RegistryStrategyOptions> _options;

    public NpgsqlRegistryStrategy(ILogger<NpgsqlRegistryStrategy> logger,
                                  StorageDbContext context,
                                  IDateTimeProvider dateTimeProvider,
                                  IOptions<RegistryStrategyOptions> options)
    {
        _logger = logger;
        _context = context;
        _dateTimeProvider = dateTimeProvider;
        _options = options;
    }

    public async Task<Result> DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var entry = await _context.StorageRegistryEntries.FindAsync(key, cancellationToken);
        if (entry is null)
        {
            return Result.Ok();
        }

        _context.StorageRegistryEntries.Remove(entry);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result<IFileProperties>> GetAsync(string key, CancellationToken cancellationToken)
    {
        var entry = await _context.StorageRegistryEntries.FindAsync(key, cancellationToken);

        return entry is null
            ? new NotFoundError()
            : entry;
    }

    public async Task<Result<IEnumerable<IFileProperties>>> GetExpiredTempFiles(CancellationToken cancellationToken)
    {
        var refDate = _dateTimeProvider.UtcNow().AddSeconds(-_options.Value.TempFileLifespan);

        return await _context.StorageRegistryEntries
            .Where(x => x.CreatedAt <= refDate && x.Preserve == false)
            .ToListAsync(cancellationToken);
    }

    public async Task<Result>
        RegisterNewFileAsync(string key, IFormFile file, CancellationToken cancellationToken = default)
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
            await _context.StorageRegistryEntries.AddAsync(entry, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to register new file");
            return new Error("Failed to register new file").CausedBy(e);
        }
    }

    public async Task<Result> SetPreserveAsync(string key, bool preserve, CancellationToken cancellationToken = default)
    {
        var entry = await _context.StorageRegistryEntries.FindAsync(key, cancellationToken);

        if (entry is null)
        {
            return new NotFoundError();
        }

        entry.Preserve = preserve;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> ValidateKeyAsync(string key, CancellationToken cancellationToken = default) =>
        Result.OkIf(
            await _context.StorageRegistryEntries.Where(x => x.Key == key).AnyAsync(cancellationToken),
            "Key not found.");
}