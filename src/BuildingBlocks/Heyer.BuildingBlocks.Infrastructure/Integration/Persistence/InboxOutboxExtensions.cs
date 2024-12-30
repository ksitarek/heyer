using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

public static class InboxOutboxExtensions
{
    public static IServiceCollection AddMongoDbInboxStore<T>(this IServiceCollection services)
        where T : DbContext, IInboxContext
    {
        services.AddScoped<IInboxStore, DbContextInboxStore<T>>();

        return services;
    }

    public static IServiceCollection AddMongoDbOutboxStore<T>(this IServiceCollection services)
        where T : DbContext, IOutboxContext
    {
        services.AddScoped<IOutboxStore, DbContextOutboxStore<T>>();

        return services;
    }
}