using Microsoft.Extensions.Configuration;

namespace Heyer.Meta.DbMigrator.Providers;

internal class StorageDbConnectionStringProvider : IStorageDbConnectionStringProvider
{
    private readonly IConfiguration _configuration;

    public StorageDbConnectionStringProvider(IConfiguration configuration) => _configuration = configuration;

    public string? GetConnectionString() => _configuration["RegistryStrategy:NpgsqlRegistry:ConnectionString"];
}