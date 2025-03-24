using Hangfire;
using Heyer.BuildingBlocks.Infrastructure.Integration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using IHealthCheck = Heyer.BuildingBlocks.Infrastructure.HealthChecks.IHealthCheck;

namespace Heyer.BuildingBlocks.Infrastructure.Modules;

public abstract class ModuleInstaller : IModuleInstaller
{
    protected IConfiguration? Configuration;
    protected IEventBus? EventBus;
    protected abstract Func<IServiceScope> ScopeProvider { get; }
    public abstract void ConfigureEventBusSubscriptions(IEventBus eventBus);

    public void ConfigureHealthChecks(IHealthChecksBuilder healthChecksBuilder)
    {
        using var scope = ScopeProvider();

        var healthChecks = scope.ServiceProvider.GetServices<IHealthCheck>();

        foreach (var check in healthChecks)
        {
            var registration = new HealthCheckRegistration(
                check.Name,
                check,
                HealthStatus.Unhealthy,
                new[] { nameof(ModuleInstaller) },
                check.Timeout);

            healthChecksBuilder.Add(registration);
        }
    }

    public abstract void ConfigureServiceProvider();

    public void InstallModule(WebApplication app)
    {
        EnsureEventBusIsSet();

        EnsureConfigurationIsSet();

        ConfigureEndpoints(app);

        ConfigureEventBusSubscriptions(EventBus!);

        ConfigureScheduler(app.Services.GetRequiredService<IRecurringJobManager>());
    }

    public abstract void RegisterInGlobalContainer(IServiceCollection globalServices);

    public void SetConfiguration(IConfiguration configuration) => Configuration = configuration;
    public void SetEventBus(IEventBus eventBus) => EventBus = eventBus;

    protected abstract void ConfigureEndpoints(WebApplication app);
    protected abstract void ConfigureScheduler(IRecurringJobManager recurringJobManager);

    protected void EnsureConfigurationIsSet()
    {
        if (Configuration == null)
        {
            throw new InvalidOperationException("Configuration is not set");
        }
    }

    protected void EnsureEventBusIsSet()
    {
        if (EventBus == null)
        {
            throw new InvalidOperationException("EventBus is not set");
        }
    }
}