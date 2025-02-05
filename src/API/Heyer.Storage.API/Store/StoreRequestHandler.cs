using FluentResults;
using Heyer.Storage.API.Client.PublishedLanguage;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Storage;
using MediatR;

namespace Heyer.Storage.API.Store;

public class StoreRequestHandler : IRequestHandler<StoreRequest, Result<StoreResult>>
{
    private readonly IRegistryStrategy _registryStrategy;
    private readonly IStorageStrategy _storageStrategy;

    public StoreRequestHandler(IStorageStrategy storageStrategy, IRegistryStrategy registryStrategy)
    {
        _storageStrategy = storageStrategy;
        _registryStrategy = registryStrategy;
    }

    public async Task<Result<StoreResult>> Handle(StoreRequest request, CancellationToken cancellationToken)
    {
        var key = Guid.CreateVersion7().ToString();

        await using var fileReadStream = request.File.OpenReadStream();

        var storeResult = await _storageStrategy.StoreAsync(key, fileReadStream, cancellationToken);
        if (storeResult.IsFailed)
        {
            return Result.Fail(storeResult.Errors);
        }

        var registerNewFileResult = await _registryStrategy.RegisterNewFileAsync(key, request.File, cancellationToken);
        if (registerNewFileResult.IsFailed)
        {
            await _storageStrategy.DeleteAsync(key, CancellationToken.None);
            return Result.Fail(registerNewFileResult.Errors);
        }

        return new StoreResult(key);
    }
}