using Heyer.Storage.API.Providers.Registry.MongoDB;

namespace Heyer.Storage.API.Providers.Registry;

public class RegistryStrategyOptions
{
    public enum RegistryStrategyType
    {
        Unknown,
        MongoDB
    }

    public MongoDBRegistryOptions MongoDBRegistry { get; set; } = new();
    public int TempFileLifespan { get; set; }
    public RegistryStrategyType Type { get; set; }
}