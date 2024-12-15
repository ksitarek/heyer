using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Mediator;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Type[] pipelineBehaviors)
    {
        var assembly = Assembly.GetCallingAssembly();
        
        services.AddMediatR(c => c.RegisterServicesFromAssembly(assembly));

        foreach (var pipelineBehavior in pipelineBehaviors)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), pipelineBehavior);
        }

        return services;
    }
}