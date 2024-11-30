using FluentResults;
using Heyer.Storage.API.Client.PublishedLanguage;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Storage;
using MediatR;

namespace Heyer.Storage.API.Preserve;

public class PreserveRequestHandler : IRequestHandler<PreserveRequest, Result>
{
    private readonly IStorageStrategy _storageStrategy;
    private readonly IRegistryStrategy _registryStrategy;

    public PreserveRequestHandler(IStorageStrategy storageStrategy, IRegistryStrategy registryStrategy)
    {
        _storageStrategy = storageStrategy;
        _registryStrategy = registryStrategy;
    }

    public async Task<Result> Handle(PreserveRequest request, CancellationToken cancellationToken)
    {
        var setPreserveResult = await _registryStrategy.SetPreserveAsync(request.Key, true, cancellationToken);
        if (setPreserveResult.IsFailed)
        {
            return Result.Fail(setPreserveResult.Errors);
        }

        var preserveResult = await _storageStrategy.PreserveAsync(request.Key, cancellationToken);
        if (preserveResult.IsFailed)
        {
            await _registryStrategy.SetPreserveAsync(request.Key, false, CancellationToken.None);
            return Result.Fail(preserveResult.Errors);
        }

        return Result.Ok();
    }
}