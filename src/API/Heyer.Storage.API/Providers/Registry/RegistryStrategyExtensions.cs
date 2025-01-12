using Heyer.Storage.API.Providers.Registry.MongoDB;
using Heyer.Storage.API.Providers.Registry.Npgsql;

namespace Heyer.Storage.API.Providers.Registry;

public static class RegistryStrategyExtensions
{
    public static IServiceCollection AddRegistryStrategy(this IServiceCollection services, IConfiguration configuration)
    {
        var registryStrategyOptions = new RegistryStrategyOptions();
        configuration.Bind(registryStrategyOptions);
        services.Configure<RegistryStrategyOptions>(configuration);

        switch (registryStrategyOptions.Type)
        {
            case RegistryStrategyOptions.RegistryStrategyType.MongoDB:
                services.AddMongoDBRegistryProvider(registryStrategyOptions.MongoDBRegistry);
                break;

            case RegistryStrategyOptions.RegistryStrategyType.Npgsql:
                services.AddNpgsqlRegistryProvider(registryStrategyOptions.NpgsqlRegistry);
                break;

            case RegistryStrategyOptions.RegistryStrategyType.Unknown:
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(registryStrategyOptions.Type),
                    registryStrategyOptions.Type,
                    "Unknown registry strategy type.");
        }

        return services;
    }
}