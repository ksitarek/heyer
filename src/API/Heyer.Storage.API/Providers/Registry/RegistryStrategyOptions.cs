using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Providers.Registry.SqlServer;

namespace Heyer.Storage.API.Providers.Registry;

public class RegistryStrategyOptions
{
    public enum RegistryStrategyType
    {
        Unknown,
        MongoDB,
        SqlServer
    }

    public MongoDBRegistryOptions MongoDBRegistry { get; set; } = new();
    public SqlServerRegistryOptions SqlServerRegistry { get; set; } = new();
    public int TempFileLifespan { get; set; }
    public RegistryStrategyType Type { get; set; }
}