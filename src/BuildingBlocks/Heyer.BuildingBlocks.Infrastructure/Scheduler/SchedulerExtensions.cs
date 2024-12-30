using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Scheduler;

public static class SchedulerExtensions
{
    public static IServiceCollection AddScheduler(this IServiceCollection services,
                                                  IConfiguration schedulerConfiguration)
    {
        var connectionString = schedulerConfiguration["SqlServer:ConnectionString"];

        services.AddHangfire(configuration => configuration
                                 .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                 .UseSimpleAssemblyNameTypeSerializer()
                                 .UseRecommendedSerializerSettings()
                                 .UseSqlServerStorage(connectionString,
                                                      new SqlServerStorageOptions { PrepareSchemaIfNecessary = true }));

        services.AddHangfireServer(x => { x.SchedulePollingInterval = TimeSpan.FromSeconds(1); });

        return services;
    }
}