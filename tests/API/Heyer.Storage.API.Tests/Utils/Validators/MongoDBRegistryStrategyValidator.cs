using FluentAssertions;
using Heyer.Storage.API.Providers.Registry.SqlServer;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using StorageRegistryEntry = Heyer.Storage.API.Providers.Registry.MongoDB.StorageRegistryEntry;

namespace Heyer.Storage.API.Tests.Utils.Validators;

internal class MongoDBRegistryStrategyValidator : IRegistryStrategyValidator
{
    private readonly IMongoCollection<StorageRegistryEntry> _collection;

    public MongoDBRegistryStrategyValidator(IMongoCollection<StorageRegistryEntry> collection) =>
        _collection = collection;

    public async Task ValidateFileIsNotPresent(string key)
    {
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, key);
        var result = await _collection.Find(filter).AnyAsync();

        result.Should().BeFalse();
    }

    public async Task ValidateFileIsPreserved(string key)
    {
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, key);
        var entry = await _collection.Find(filter).FirstOrDefaultAsync();
        entry.Should().NotBeNull();
        entry.Preserve.Should().BeTrue();
    }

    public async Task ValidateFilePropertiesAsync(string key,
                                                  string expectedFileName,
                                                  string expectedContentType,
                                                  int expectedSize)
    {
        var filter = Builders<StorageRegistryEntry>.Filter.Eq(x => x.Key, key);
        var entry = await _collection.Find(filter).FirstAsync();

        entry.FileName.Should().Be(expectedFileName);
        entry.ContentType.Should().Be(expectedContentType);
        entry.Size.Should().Be(expectedSize);
    }
}

internal class SqlServerRegistryStrategyValidator : IRegistryStrategyValidator
{
    private readonly StorageDbContext _context;

    public SqlServerRegistryStrategyValidator(StorageDbContext context) => _context = context;

    public async Task ValidateFileIsNotPresent(string key)
    {
        var result = await _context.StorageRegistryEntries.AnyAsync(x => x.Key == key);
        result.Should().BeFalse();
    }

    public async Task ValidateFileIsPreserved(string key)
    {
        var result = await _context.StorageRegistryEntries.FirstAsync(x => x.Key == key);
        result.Preserve.Should().BeTrue();
    }

    public async Task ValidateFilePropertiesAsync(string key,
                                                  string expectedFileName,
                                                  string expectedContentType,
                                                  int expectedSize)
    {
        var result = await _context.StorageRegistryEntries.FirstAsync(x => x.Key == key);
        result.Should().NotBeNull();
        result.FileName.Should().Be(expectedFileName);
        result.ContentType.Should().Be(expectedContentType);
        result.Size.Should().Be(expectedSize);
    }
}