using Heyer.Storage.API.Providers.Registry.Npgsql;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Heyer.Storage.API.Tests.Utils.Validators;

internal class NpgsqlRegistryStrategyValidator : IRegistryStrategyValidator
{
    private readonly StorageDbContext _context;

    public NpgsqlRegistryStrategyValidator(StorageDbContext context) => _context = context;

    public async Task ValidateFileIsNotPresent(string key)
    {
        var result = await _context.StorageRegistryEntries.AnyAsync(x => x.Key == key);
        result.ShouldBeFalse();
    }

    public async Task ValidateFileIsPreserved(string key)
    {
        var result = await _context.StorageRegistryEntries.FirstAsync(x => x.Key == key);
        result.Preserve.ShouldBeTrue();
    }

    public async Task ValidateFilePropertiesAsync(string key,
                                                  string expectedFileName,
                                                  string expectedContentType,
                                                  int expectedSize)
    {
        var result = await _context.StorageRegistryEntries.FirstAsync(x => x.Key == key);
        result.ShouldNotBeNull();
        result.FileName.ShouldBe(expectedFileName);
        result.ContentType.ShouldBe(expectedContentType);
        result.Size.ShouldBe(expectedSize);
    }
}