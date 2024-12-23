using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Modules;

public static class ModulesExtensions
{
    public static IServiceCollection AddModule<TModuleInterface, TModule>(
        this IServiceCollection services,
        TModule module)
        where TModuleInterface : class, IModuleInstaller
        where TModule : class, TModuleInterface
    {
        services.AddSingleton<TModuleInterface, TModule>(_ => module);
        services.AddSingleton<IModuleInstaller, TModule>(_ => module);

        return services;
    }

    public static IApplicationBuilder UseModules(this WebApplication app)
    {
        var modules = app.Services.GetServices<IModuleInstaller>();

        foreach (var module in modules)
        {
            module.ConfigureModule(app);
        }

        return app;
    }
}