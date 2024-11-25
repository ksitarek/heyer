using Heyer.Storage.API.Providers.Storage.Filesystem;

namespace Heyer.Storage.API.Providers.Storage;

public class StorageStrategyOptions
{
    public StorageStrategyType Type { get; set; }
    public FilesystemStorageOptions FilesystemStorage { get; set; } = new();
    
    public enum StorageStrategyType
    {
        Unknown,
        Filesystem
    }
}