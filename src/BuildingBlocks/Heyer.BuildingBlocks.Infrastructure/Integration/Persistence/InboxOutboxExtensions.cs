using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

public static class InboxOutboxExtensions
{
    public static IServiceCollection AddMongoDbInboxStore(this IServiceCollection services)
    {
        // in multitenant environment this MUST be scoped, not singleton, even though MongoDbInboxStore would allow it
        services.AddScoped<IInboxStore, MongoDbInboxStore>();

        return services;
    }

    public static IServiceCollection AddMongoDbOutboxStore(this IServiceCollection services)
    {
        // in multitenant environment this MUST be scoped, not singleton, even though MongoDbOutboxStore would allow it
        services.AddScoped<IOutboxStore, MongoDbOutboxStore>();

        return services;
    }
}