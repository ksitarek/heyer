using System.Reflection;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Mediator;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services,
                                                 IModule[] modules,
                                                 params Type[] pipelineBehaviors)
    {
        var assemblies = modules
            .Select(x => x.ModuleApplicationAssembly)
            .Append(Assembly.GetCallingAssembly())
            .ToArray();

        return services.AddMediator(assemblies, pipelineBehaviors);
    }

    public static IServiceCollection AddMediator(this IServiceCollection services, params Type[] pipelineBehaviors) =>
        services.AddMediator([Assembly.GetCallingAssembly()], pipelineBehaviors);

    private static IServiceCollection AddMediator(this IServiceCollection services,
                                                  Assembly[] assemblies,
                                                  Type[] pipelineBehaviors)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssemblies(assemblies));

        foreach (var pipelineBehavior in pipelineBehaviors)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), pipelineBehavior);
        }

        return services;
    }
}