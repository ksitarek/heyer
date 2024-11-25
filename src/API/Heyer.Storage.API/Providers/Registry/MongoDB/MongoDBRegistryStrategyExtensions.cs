using Microsoft.Extensions.Options;

namespace Heyer.Storage.API.Providers.Registry.MongoDB;

internal static class MongoDBRegistryStrategyExtensions
{
    public static IServiceCollection AddMongoDBRegistryProvider(this IServiceCollection services, MongoDBRegistryOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddSingleton<IRegistryStrategy, MongoDBRegistry>();
        return services;
    }
}