namespace Heyer.Storage.API.Providers.Registry;

public interface IRegistryStrategy
{
    Task RegisterNewFileAsync(string key, IFormFile file, CancellationToken cancellationToken = default);
    Task Preserve(string key, CancellationToken cancellationToken = default);
}