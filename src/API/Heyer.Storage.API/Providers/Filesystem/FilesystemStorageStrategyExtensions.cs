using Microsoft.Extensions.Options;

namespace Heyer.Storage.API.Providers.Filesystem;

internal static class FilesystemStorageStrategyExnuktensions
{
    public static IServiceCollection AddFilesystemProvider(this IServiceCollection services, FilesystemStorageOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddSingleton<IStorageStrategy, FilesystemStorageStrategy>();
        return services;
    }
}