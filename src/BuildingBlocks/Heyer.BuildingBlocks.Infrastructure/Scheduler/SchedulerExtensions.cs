using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Scheduler;

public static class SchedulerExtensions
{
    public static IServiceCollection AddScheduler(this IServiceCollection services,
                                                  IConfiguration schedulerConfiguration)
    {
        var connectionString = schedulerConfiguration["Npgsql:ConnectionString"];

        services.AddHangfire(configuration => configuration
                                 .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                 .UseSimpleAssemblyNameTypeSerializer()
                                 .UseRecommendedSerializerSettings()
                                 .UsePostgreSqlStorage(o => { o.UseNpgsqlConnection(connectionString); }));

        services.AddHangfireServer(x => { x.SchedulePollingInterval = TimeSpan.FromSeconds(1); });

        return services;
    }
}