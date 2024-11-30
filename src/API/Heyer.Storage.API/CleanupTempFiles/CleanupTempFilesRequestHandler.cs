using FluentResults;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Providers.Storage;
using MediatR;

namespace Heyer.Storage.API.CleanupTempFiles;

public class CleanupTempFilesRequestHandler : IRequestHandler<CleanupTempFilesRequest, Result>
{
    private readonly ILogger<CleanupTempFilesRequestHandler> _logger;
    private readonly IRegistryStrategy _registryStrategy;
    private readonly IStorageStrategy _storageStrategy;

    public CleanupTempFilesRequestHandler(
        ILogger<CleanupTempFilesRequestHandler> logger,
        IRegistryStrategy registryStrategy,
        IStorageStrategy storageStrategy)
    {
        _logger = logger;
        _registryStrategy = registryStrategy;
        _storageStrategy = storageStrategy;
    }

    public async Task<Result> Handle(CleanupTempFilesRequest request, CancellationToken cancellationToken)
    {
        var expiredTempFiles = await _registryStrategy.GetExpiredTempFiles(cancellationToken);

        if (expiredTempFiles.IsFailed)
        {
            _logger.LogError("Failed to retrieve expired temp files: {errors}", expiredTempFiles.Errors);
            return Result.Fail("Failed to retrieve expired temp files.").WithErrors(expiredTempFiles.Errors);
        }

        var result = Result.Ok();

        foreach (var expiredTempFile in expiredTempFiles.ValueOrDefault)
        {
            var deleteResult = await DeleteTempFile(expiredTempFile, cancellationToken);

            if (deleteResult.IsFailed)
            {
                _logger.LogError("Failed to delete temp file with key {key}: {errors}",
                                 expiredTempFile.Key,
                                 deleteResult.Errors);

                result.WithError(new Error($"Failed to delete temp file with key {expiredTempFile.Key}.")
                                     .CausedBy(deleteResult.Errors));
            }
            else
            {
                _logger.LogTrace("Deleted temp file successfully {key}.", expiredTempFile.Key);
                result.WithSuccess($"Deleted temp file successfully {expiredTempFile.Key}.");
            }
        }

        return result;
    }

    private async Task<Result> DeleteTempFile(IFileProperties file, CancellationToken cancellationToken)
    {
        var storageResult = await _storageStrategy.DeleteAsync(file.Key, cancellationToken);
        if (storageResult.IsFailed)
        {
            return Result.Fail(storageResult.Errors);
        }

        var registryResult = await _registryStrategy.DeleteAsync(file.Key, cancellationToken);
        if (registryResult.IsFailed)
        {
            return Result.Fail(registryResult.Errors);
        }

        return Result.Ok();
    }
}