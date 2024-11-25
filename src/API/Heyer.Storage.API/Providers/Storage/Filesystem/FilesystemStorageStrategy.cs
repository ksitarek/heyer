using Microsoft.Extensions.Options;

namespace Heyer.Storage.API.Providers.Storage.Filesystem;

internal class FilesystemStorageStrategy : IStorageStrategy
{
    private readonly string _rootPath;

    public FilesystemStorageStrategy(IOptions<FilesystemStorageOptions> options)
    {
        _rootPath = options.Value.RootPath;
        EnsureRootPathExists();
    }

    private void EnsureRootPathExists()
    {
        if (!Directory.Exists(_rootPath))
        {
            Directory.CreateDirectory(_rootPath);
        }
    }

    public async Task StoreAsync(string key, Stream stream, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(_rootPath, key);

        if (File.Exists(path))
        {
            throw new InvalidOperationException("File already exists.");
        }
        
        await using var fileStream = File.Create(path);
        stream.Seek(0, SeekOrigin.Begin);
        await stream.CopyToAsync(fileStream, cancellationToken);
    }

    public Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(_rootPath, key);
        File.Delete(path);
        return Task.CompletedTask;
    }

    public Task<Stream> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(_rootPath, key);
        
        try
        {
            return Task.FromResult<Stream>(File.OpenRead(path));
        }
        catch (FileNotFoundException e)
        {
            throw new FileNotFoundException("File not found.", e);
        }
    }
}