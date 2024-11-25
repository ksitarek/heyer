using FluentResults;
using Heyer.Storage.API.Providers;
using Heyer.Storage.API.Providers.Storage;
using MediatR;

namespace Heyer.Storage.API.Endpoints.Store;

public class StoreRequestHandler : IRequestHandler<StoreRequest, Result<StoreResult>>
{
    private readonly IStorageStrategy _storageStrategy;

    public StoreRequestHandler(IStorageStrategy storageStrategy)
    {
        _storageStrategy = storageStrategy;
    }
    
    public async Task<Result<StoreResult>> Handle(StoreRequest request, CancellationToken cancellationToken)
    {
        var key = Guid.NewGuid().ToString();

        await using var fileReadStream = request.File.OpenReadStream();
        
        await _storageStrategy.StoreAsync(key, fileReadStream, cancellationToken);
        
        // TODO add db record

        return new StoreResult(key);
    }
}