using FluentAssertions;
using Heyer.Storage.API.Providers.Registry.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Storage.API.Tests.Utils.Validators;

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