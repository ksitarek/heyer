using FluentResults;
using Heyer.Storage.API.Client.PublishedLanguage;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Storage;
using MediatR;

namespace Heyer.Storage.API.Endpoints.Delete;

public class DeleteRequestHandler : IRequestHandler<DeleteRequest, Result>
{
    private readonly IRegistryStrategy _registryStrategy;
    private readonly IStorageStrategy _storageStrategy;

    public DeleteRequestHandler(IRegistryStrategy registryStrategy, IStorageStrategy storageStrategy)
    {
        _registryStrategy = registryStrategy;
        _storageStrategy = storageStrategy;
    }
    
    public async Task<Result> Handle(DeleteRequest request, CancellationToken cancellationToken)
    {
        var storageResult = await _storageStrategy.DeleteAsync(request.Key, cancellationToken);
        if (storageResult.IsFailed)
        {
            return Result.Fail(storageResult.Errors);
        }
        
        var registryResult = await _registryStrategy.DeleteAsync(request.Key, cancellationToken);
        if (registryResult.IsFailed)
        {
            return Result.Fail(registryResult.Errors);
        }
        
        return Result.Ok();
    }
}