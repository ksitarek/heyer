using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Modules;

public static class ModulesExtensions
{
    public static IApplicationBuilder UseModules(this WebApplication app)
    {
        var modules = app.Services.GetServices<IModuleInstaller>();

        foreach (var module in modules)
        {
            module.InstallModule(app);
        }

        return app;
    }
}