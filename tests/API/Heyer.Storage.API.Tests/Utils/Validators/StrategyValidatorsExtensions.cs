using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Providers.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Heyer.Storage.API.Tests.Utils.Validators;

internal static class StrategyValidatorsExtensions
{
    internal static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddStorageStrategyValidator()
            .AddRegistryStrategyValidator();
    }

    private static IServiceCollection AddRegistryStrategyValidator(this IServiceCollection services)
    {
        services.AddSingleton<IRegistryStrategyValidator>((sp) =>
        {
            var registryStrategyOptions = sp.GetRequiredService<IOptions<RegistryStrategyOptions>>().Value;

            switch (registryStrategyOptions.Type)
            {
                case RegistryStrategyOptions.RegistryStrategyType.MongoDB:
                    return new MongoDBRegistryStrategyValidator(
                        sp.GetRequiredService<IMongoCollection<StorageRegistryEntry>>()
                    );
                case RegistryStrategyOptions.RegistryStrategyType.Unknown:
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(registryStrategyOptions.Type),
                        registryStrategyOptions.Type,
                        "Unknown registry strategy type.");
            }
        });

        return services;
    }

    private static IServiceCollection AddStorageStrategyValidator(this IServiceCollection services)
    {
        services.AddSingleton<IStorageStrategyValidator>((sp) =>
        {
            var storageStrategyOptions = sp.GetRequiredService<IOptions<StorageStrategyOptions>>().Value;

            switch (storageStrategyOptions.Type)
            {
                case StorageStrategyOptions.StorageStrategyType.Filesystem:
                    return new FilesystemStorageStrategyValidator(
                        Options.Create(storageStrategyOptions.FilesystemStorage));
                case StorageStrategyOptions.StorageStrategyType.Unknown:
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(storageStrategyOptions.Type),
                        storageStrategyOptions.Type,
                        "Unknown storage strategy type.");
            }
        });

        return services;
    }
}