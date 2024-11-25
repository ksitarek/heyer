using Microsoft.Extensions.Options;

namespace Heyer.Storage.API.Providers.Registry.MongoDB;

public class MongoDBRegistry : IRegistryStrategy
{
    public MongoDBRegistry(IOptions<MongoDBRegistryOptions> options)
    {
        
    }
    
    public Task RegisterNewFileAsync(string key, IFormFile file)
    {
        throw new NotImplementedException();
    }
}