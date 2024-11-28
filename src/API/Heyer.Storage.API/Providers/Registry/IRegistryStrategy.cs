using FluentResults;
using Heyer.Storage.API.Providers.Registry.MongoDB;

namespace Heyer.Storage.API.Providers.Registry;

public interface IRegistryStrategy
{
    Task<Result> RegisterNewFileAsync(string key, IFormFile file, CancellationToken cancellationToken = default);
    Task<Result> SetPreserveAsync(string key, bool preserve, CancellationToken cancellationToken = default);
    Task<Result> ValidateKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<Result<IFileProperties>> GetAsync(string key, CancellationToken cancellationToken);
}