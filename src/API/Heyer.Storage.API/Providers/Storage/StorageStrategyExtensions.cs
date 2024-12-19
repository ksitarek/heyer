using Heyer.Storage.API.Providers.Storage.Filesystem;

namespace Heyer.Storage.API.Providers.Storage;

public static class StorageStrategyExtensions
{
    public static IServiceCollection AddStorageStrategy(this IServiceCollection services, IConfiguration configuration)
    {
        var storageStrategyOptions = new StorageStrategyOptions();
        configuration.Bind(storageStrategyOptions);
        services.Configure<StorageStrategyOptions>(configuration);

        switch (storageStrategyOptions.Type)
        {
            case StorageStrategyOptions.StorageStrategyType.Filesystem:
                services.AddFilesystemProvider(storageStrategyOptions.FilesystemStorage);
                break;
            case StorageStrategyOptions.StorageStrategyType.Unknown:
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(storageStrategyOptions.Type),
                    storageStrategyOptions.Type,
                    "Unknown storage strategy type.");
        }


        return services;
    }
}