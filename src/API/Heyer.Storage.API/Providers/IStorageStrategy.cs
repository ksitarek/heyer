using Microsoft.Extensions.Options;

namespace Heyer.Storage.API.Providers;

public interface IStorageStrategy
{
    public Task StoreAsync(string key, Stream stream, CancellationToken cancellationToken = default);
    public Task DeleteAsync(string key, CancellationToken cancellationToken = default);
    public Task<Stream> GetAsync(string key, CancellationToken cancellationToken = default);
}