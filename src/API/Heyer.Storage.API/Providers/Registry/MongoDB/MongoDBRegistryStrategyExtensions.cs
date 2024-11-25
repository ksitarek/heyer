using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Heyer.Storage.API.Providers.Registry.MongoDB;

internal static class MongoDBRegistryStrategyExtensions
{
    public static IServiceCollection AddMongoDBRegistryProvider(this IServiceCollection services, MongoDBRegistryOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddSingleton<IRegistryStrategy, MongoDBRegistryStrategy>();

        services.ConfigureMongoDB(options);

        return services;
    }

    private static IServiceCollection ConfigureMongoDB(this IServiceCollection services, MongoDBRegistryOptions options)
    {
        services.AddSingleton<MongoClient>(_ => new MongoClient(options.ConnectionString));
        
        services.AddSingleton<IMongoDatabase>(p =>
        {
            var client = p.GetRequiredService<MongoClient>();
            return client.GetDatabase(options.DatabaseName);
        });

        services.AddSingleton<IMongoCollection<StorageRegistryEntry>>(p =>
        {
            var database = p.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<StorageRegistryEntry>(options.CollectionName);
        });

        return services;
    }
}