using Heyer.BuildingBlocks.Infrastructure.Integration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Modules;

public interface IModuleInstaller
{
    void ConfigureEventBusSubscriptions(IEventBus eventBus);
    void ConfigureHealthChecks(IHealthChecksBuilder healthChecksBuilder);
    void ConfigureServiceProvider();
    void InstallModule(WebApplication app);

    void RegisterInGlobalContainer(IServiceCollection globalServices);
    void SetConfiguration(IConfiguration configuration);
    void SetEventBus(IEventBus eventBus);
}