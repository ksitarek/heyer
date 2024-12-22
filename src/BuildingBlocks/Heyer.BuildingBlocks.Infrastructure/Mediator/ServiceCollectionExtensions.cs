using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Mediator;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Type[] pipelineBehaviors) =>
        services.AddMediator(Assembly.GetCallingAssembly(), pipelineBehaviors);

    public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly, params Type[] pipelineBehaviors) =>
        services.AddMediator([assembly], pipelineBehaviors);

    private static IServiceCollection AddMediator(this IServiceCollection services,
                                                  Assembly[] assemblies,
                                                  Type[] pipelineBehaviors)
    {
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssemblies(assemblies);

            foreach (var pipelineBehavior in pipelineBehaviors)
            {
                c.AddOpenBehavior(pipelineBehavior);
            }
        });

        return services;
    }
}