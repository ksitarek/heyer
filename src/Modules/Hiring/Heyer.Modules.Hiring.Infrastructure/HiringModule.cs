using System.Reflection;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.Hiring.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.Hiring.Infrastructure;

public class HiringModule : ModuleRunner, IHiringModule, IModuleInstaller
{
    public HiringModule(IConfiguration configuration)
    {
        var services = new ServiceCollection();

        ConfigureServices(configuration, services);

        HiringModuleCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }

    public Assembly ModuleApplicationAssembly => typeof(HiringEndpointsConfiguration).Assembly;

    public override Func<IServiceScope> ScopeProvider => HiringModuleCompositionRoot.CreateScope;

    public void ConfigureModule(WebApplication app) => HiringEndpointsConfiguration.MapEndpoints(app);

    private void ConfigureServices(IConfiguration configuration, ServiceCollection services)
    {
    }
}