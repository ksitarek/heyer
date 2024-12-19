using Microsoft.Extensions.Options;

namespace Heyer.Storage.API.Providers.Storage.Filesystem;

internal static class FilesystemStorageStrategyExtensions
{
    public static IServiceCollection AddFilesystemProvider(this IServiceCollection services,
                                                           FilesystemStorageOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddSingleton<IStorageStrategy, FilesystemStorageStrategy>();
        return services;
    }
}