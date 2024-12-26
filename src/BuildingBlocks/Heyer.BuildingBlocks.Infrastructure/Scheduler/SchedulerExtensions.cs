using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Heyer.BuildingBlocks.Infrastructure.Scheduler;

public static class SchedulerExtensions
{
    public static IServiceCollection AddScheduler(this IServiceCollection services,
                                                  IConfiguration schedulerConfiguration)
    {
        var connectionString = schedulerConfiguration["MongoDb:ConnectionString"];
        var databaseName = schedulerConfiguration["MongoDb:DatabaseName"];

        var mongoClient = new MongoClient(connectionString);

        services.AddHangfire(configuration => configuration
                                 .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                                 .UseSimpleAssemblyNameTypeSerializer()
                                 .UseRecommendedSerializerSettings()
                                 .UseMongoStorage(mongoClient,
                                                  databaseName,
                                                  new MongoStorageOptions
                                                  {
                                                      Prefix = "hangfire.mongo",
                                                      CheckConnection = true,
                                                      CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.Watch,
                                                      QueuePollInterval = TimeSpan.FromSeconds(1),
                                                      MigrationOptions = new MongoMigrationOptions
                                                      {
                                                          MigrationStrategy =
                                                              new MigrateMongoMigrationStrategy()
                                                      }
                                                  }));

        services.AddHangfireServer(x => { x.SchedulePollingInterval = TimeSpan.FromSeconds(1); });

        return services;
    }
}