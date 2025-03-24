using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Integration;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.BuildingBlocks.Infrastructure.Scheduler;
using Heyer.BuildingBlocks.Json;
using Serilog;

namespace Heyer.API.Host;

internal class HostBuilder
{
    private readonly WebApplicationBuilder _builder;

    private readonly IEventBus _eventBus = new InProcessEventBus();

    private readonly IHealthChecksBuilder _healthChecksBuilder;

    public HostBuilder(WebApplicationBuilder builder)
    {
        _builder = builder;

        _builder.Services.AddEndpointsApiExplorer();
        _builder.Services.AddAuthenticationAndAuthorization(_builder.Configuration.GetSection("Jwt"));
        _builder.Services.AddScheduler(builder.Configuration.GetSection("Scheduler"));
        _builder.Services.AddCors(builder.Configuration.GetSection("Cors"));
        _builder.Services.ConfigureJson();

        _healthChecksBuilder = _builder.Services.AddHealthChecks();
    }

    public HostBuilder AddModule<TInterface, TImplementation>()
        where TImplementation : class, TInterface, new()
        where TInterface : class, IModuleInstaller
    {
        var module = new TImplementation();

        if (module is null)
        {
            throw new Exception($"Unable to create instance of module {typeof(TImplementation)}");
        }

        module.SetEventBus(_eventBus);
        module.SetConfiguration(_builder.Configuration);

        module.ConfigureServiceProvider();

        module.ConfigureHealthChecks(_healthChecksBuilder);

        _builder.Services.AddSingleton<TInterface, TImplementation>(_ => module);
        _builder.Services.AddSingleton<IModuleInstaller, TImplementation>(_ => module);

        module.RegisterInGlobalContainer(_builder.Services);

        return this;
    }

    public Host Build() => new(_builder.Build());

    public HostBuilder ConfigureLogging()
    {
        _builder.Services.AddSerilog((sp, lc) => lc
                                         .ReadFrom.Configuration(_builder.Configuration)
                                         .ReadFrom.Services(sp));

        return this;
    }
}