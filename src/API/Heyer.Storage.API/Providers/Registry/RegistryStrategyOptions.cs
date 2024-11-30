using Heyer.Storage.API.Providers.Registry.MongoDB;

namespace Heyer.Storage.API.Providers.Registry;

public class RegistryStrategyOptions
{
    public RegistryStrategyType Type { get; set; }
    public int TempFileLifespan { get; set; }
    public MongoDBRegistryOptions MongoDBRegistry { get; set; } = new();

    public enum RegistryStrategyType
    {
        Unknown,
        MongoDB
    }
}