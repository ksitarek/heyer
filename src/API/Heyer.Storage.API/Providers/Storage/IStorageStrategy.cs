using FluentResults;

namespace Heyer.Storage.API.Providers.Storage;

public interface IStorageStrategy
{
    public Task<Result> DeleteAsync(string key, CancellationToken cancellationToken = default);
    public Task<Result<Stream>> GetAsync(string key, CancellationToken cancellationToken = default);
    public Task<Result<long>> GetAvailableFreeSpaceAsync(CancellationToken cancellationToken = default);
    public Task<Result> PreserveAsync(string key, CancellationToken cancellationToken = default);
    public Task<Result> StoreAsync(string key, Stream stream, CancellationToken cancellationToken = default);
}