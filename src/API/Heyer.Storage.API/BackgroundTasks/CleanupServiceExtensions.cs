namespace Heyer.Storage.API.BackgroundTasks;

public static class CleanupServiceExtensions
{
    public static IServiceCollection AddCleanupService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CleanupServiceOptions>(configuration);
        services.AddHostedService<CleanupService>();
        return services;
    }
}