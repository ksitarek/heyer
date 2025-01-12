using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Providers.Registry.Npgsql;

namespace Heyer.Storage.API.Providers.Registry;

public class RegistryStrategyOptions
{
    public enum RegistryStrategyType
    {
        Unknown,
        MongoDB,
        Npgsql
    }

    public MongoDBRegistryOptions MongoDBRegistry { get; set; } = new();
    public NpgsqlRegistryOptions NpgsqlRegistry { get; set; } = new();
    public int TempFileLifespan { get; set; }
    public RegistryStrategyType Type { get; set; }
}