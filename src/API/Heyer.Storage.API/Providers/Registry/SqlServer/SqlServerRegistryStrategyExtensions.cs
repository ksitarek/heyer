using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Heyer.Storage.API.Providers.Registry.SqlServer;

internal static class SqlServerRegistryStrategyExtensions
{
    public static IServiceCollection AddSqlServerRegistryProvider(this IServiceCollection services,
                                                                  SqlServerRegistryOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddScoped<IRegistryStrategy, SqlServerRegistryStrategy>();

        services.AddDbContext<StorageDbContext>(o => o.UseSqlServer(options.ConnectionString));

        return services;
    }
}