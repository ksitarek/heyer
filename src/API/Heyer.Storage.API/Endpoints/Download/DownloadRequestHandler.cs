using FluentResults;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Storage;
using MediatR;

namespace Heyer.Storage.API.Endpoints.Download;

public class DownloadRequestHandler : IRequestHandler<DownloadRequest, Result<DownloadResponse>>
{
    private readonly IRegistryStrategy _registryStrategy;
    private readonly IStorageStrategy _storageStrategy;

    public DownloadRequestHandler(IRegistryStrategy registryStrategy, IStorageStrategy storageStrategy)
    {
        _registryStrategy = registryStrategy;
        _storageStrategy = storageStrategy;
    }
    
    public async Task<Result<DownloadResponse>> Handle(DownloadRequest request, CancellationToken cancellationToken)
    {
        var entry = await _registryStrategy.GetAsync(request.Key, cancellationToken);
        if (entry.IsFailed)
            return Result.Fail(entry.Errors);
        
        var file = await _storageStrategy.GetAsync(request.Key, cancellationToken);
        if(file.IsFailed)
            return Result.Fail(file.Errors);
        
        return new DownloadResponse(entry.Value.FileName, entry.Value.ContentType, file.Value);
    }
}