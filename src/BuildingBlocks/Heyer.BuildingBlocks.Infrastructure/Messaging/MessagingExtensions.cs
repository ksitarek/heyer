using System.Reflection;
using Heyer.BuildingBlocks.Application.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddDomainEventDispatcher(this IServiceCollection services,
                                                              Assembly assembly)
    {
        var notificationsRegistry = new DomainEventNotificationsRegistry();

        notificationsRegistry.LoadFromAssembly(assembly);

        return services.AddSingleton<IDomainEventNotificationsRegistry>(notificationsRegistry)
            .AddScoped<IDomainEventDispatcher, DomainEventDispatcher>()
            .AddScoped<IDomainEventsAccessor, DomainEventsAccessor>();
    }
}