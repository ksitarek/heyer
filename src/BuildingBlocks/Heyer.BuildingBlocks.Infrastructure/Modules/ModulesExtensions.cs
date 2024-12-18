using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Heyer.BuildingBlocks.Infrastructure.Modules;

public static class ModulesExtensions
{
    public static IHostApplicationBuilder AddModules(this IHostApplicationBuilder applicationBuilder,
                                                     IEnumerable<IModule> modules)
    {
        foreach (var module in modules)
        {
            module.ConfigureDependencyInjection(applicationBuilder.Services);
        }

        return applicationBuilder;
    }

    public static IApplicationBuilder UseModules(this WebApplication app, IEnumerable<IModule> modules)
    {
        foreach (var module in modules)
        {
            module.ConfigureModule(app);
        }

        return app;
    }
}