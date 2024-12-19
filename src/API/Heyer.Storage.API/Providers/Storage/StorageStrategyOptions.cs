using Heyer.Storage.API.Providers.Storage.Filesystem;

namespace Heyer.Storage.API.Providers.Storage;

public class StorageStrategyOptions
{
    public enum StorageStrategyType
    {
        Unknown,
        Filesystem
    }

    public FilesystemStorageOptions FilesystemStorage { get; set; } = new();
    public StorageStrategyType Type { get; set; }
}