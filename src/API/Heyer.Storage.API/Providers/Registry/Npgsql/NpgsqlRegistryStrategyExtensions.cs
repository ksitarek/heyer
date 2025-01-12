using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Heyer.Storage.API.Providers.Registry.Npgsql;

internal static class NpgsqlRegistryStrategyExtensions
{
    public static IServiceCollection AddNpgsqlRegistryProvider(this IServiceCollection services,
                                                               NpgsqlRegistryOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddScoped<IRegistryStrategy, NpgsqlRegistryStrategy>();

        services.AddDbContext<StorageDbContext>(o => o.UseNpgsql(options.ConnectionString));

        return services;
    }
}