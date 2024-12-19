using FluentResults;
using Heyer.BuildingBlocks.Application.Results;
using Microsoft.Extensions.Options;

namespace Heyer.Storage.API.Providers.Storage.Filesystem;

internal class FilesystemStorageStrategy : IStorageStrategy
{
    private readonly ILogger<FilesystemStorageStrategy> _logger;
    private readonly string _rootPath;

    public FilesystemStorageStrategy(IOptions<FilesystemStorageOptions> options,
                                     ILogger<FilesystemStorageStrategy> logger)
    {
        _logger = logger;
        _rootPath = options.Value.RootPath;
        EnsureRootPathExists();
    }

    public Task<Result> DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var path = Path.Combine(_rootPath, key);
            File.Delete(path);
            return Task.FromResult(Result.Ok());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete file.");
            return Task.FromResult(Result.Fail(new Error("Failed to delete file.").CausedBy(e)));
        }
    }

    public Task<Result<Stream>> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(_rootPath, key);

        try
        {
            return Task.FromResult<Result<Stream>>(File.OpenRead(path));
        }
        catch (FileNotFoundException e)
        {
            _logger.LogError(e, "File not found.");
            return Task.FromResult(Result.Fail<Stream>(new NotFoundError().CausedBy(e)));
        }
    }

    public Task<Result> PreserveAsync(string key, CancellationToken cancellationToken = default) =>
        Task.FromResult(Result.Ok());

    public async Task<Result> StoreAsync(string key, Stream stream, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(_rootPath, key);

        if (File.Exists(path))
        {
            _logger.LogError("File already exists.");
            return Result.Fail("File already exists.");
        }

        try
        {
            await using var fileStream = File.Create(path);
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(fileStream, cancellationToken);

            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create file.");
            return new Error("Failed to create file.").CausedBy(e);
        }
    }

    private void EnsureRootPathExists()
    {
        if (!Directory.Exists(_rootPath))
        {
            Directory.CreateDirectory(_rootPath);
        }
    }
}