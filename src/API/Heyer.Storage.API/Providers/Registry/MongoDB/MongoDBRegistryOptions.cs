namespace Heyer.Storage.API.Providers.Registry.MongoDB;

public class MongoDBRegistryOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string CollectionName { get; set; } = string.Empty;
}