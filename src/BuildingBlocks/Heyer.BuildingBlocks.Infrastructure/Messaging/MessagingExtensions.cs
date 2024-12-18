using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddDomainEventDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IDomainEventsAccessor, DomainEventsAccessor>();
        return services;
    }
}