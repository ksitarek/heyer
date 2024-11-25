using FileSignatures;

namespace Heyer.Storage.API.Providers.Registry;

public interface IRegistryStrategy
{
    Task RegisterNewFileAsync(string key, IFormFile file);
}