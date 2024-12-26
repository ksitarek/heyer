using Heyer.API.Client;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Integration;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.BuildingBlocks.Infrastructure.Scheduler;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using IHealthCheck = Heyer.BuildingBlocks.Infrastructure.HealthChecks.IHealthCheck;

namespace Heyer.API.Host;

internal class HostBuilder
{
    private readonly WebApplicationBuilder _builder;

    private readonly IEventBus _eventBus = new InProcessEventBus();
    private readonly List<ModuleRunner> _modules = new();

    public HostBuilder(WebApplicationBuilder builder)
    {
        _builder = builder;

        _builder.Services.AddEndpointsApiExplorer();
        _builder.Services.AddAuthenticationAndAuthorization(_builder.Configuration.GetSection("Jwt"));
        _builder.Services.AddScheduler(builder.Configuration.GetSection("Scheduler"));
        _builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy =
                ApiClientFactory.SerializerOptions.PropertyNamingPolicy;

            options.SerializerOptions.DefaultIgnoreCondition =
                ApiClientFactory.SerializerOptions.DefaultIgnoreCondition;

            foreach (var converter in ApiClientFactory.SerializerOptions.Converters)
            {
                options.SerializerOptions.Converters.Add(converter);
            }
        });
    }

    public HostBuilder AddModule<TInterface, TImplementation>()
        where TImplementation : ModuleRunner, TInterface, IModuleInstaller
        where TInterface : class, IModuleInstaller
    {
        var module = Activator.CreateInstance(typeof(TImplementation), _builder.Configuration, _eventBus)
            as TImplementation;

        if (module is null)
        {
            throw new Exception($"Unable to create instance of module {typeof(TImplementation)}");
        }

        _modules.Add(module);

        _builder.Services.AddModule<TInterface, TImplementation>(module);

        return this;
    }

    public Host Build() => new(_builder.Build());

    public HostBuilder ConfigureHealthChecks()
    {
        var healthChecksBuilder = _builder.Services.AddHealthChecks();

        foreach (var module in _modules)
        {
            using var moduleScope = module.ScopeProvider.Invoke();

            var moduleHealthChecks = moduleScope.ServiceProvider.GetServices<IHealthCheck>();
            foreach (var check in moduleHealthChecks)
            {
                healthChecksBuilder.Add(
                    new HealthCheckRegistration(
                        check.Name,
                        check,
                        HealthStatus.Unhealthy,
                        [
                            module.GetType().Name
                        ],
                        check.Timeout));
            }
        }

        return this;
    }

    public HostBuilder ConfigureLogging()
    {
        _builder.Services.AddSerilog((sp, lc) => lc
                                         .ReadFrom.Configuration(_builder.Configuration)
                                         .ReadFrom.Services(sp),
                                     true);

        return this;
    }
}