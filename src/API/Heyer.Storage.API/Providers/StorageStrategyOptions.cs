using Heyer.Storage.API.Providers.Filesystem;

namespace Heyer.Storage.API.Providers;

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