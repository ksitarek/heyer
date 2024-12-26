using Heyer.BuildingBlocks.Application.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddDomainEventDispatcher(this IServiceCollection services,
                                                              IDomainEventNotificationsRegistry
                                                                  notificationsRegistry) =>
        services.AddSingleton(notificationsRegistry)
            .AddScoped<IDomainEventDispatcher, DomainEventDispatcher>()
            .AddScoped<IDomainEventsAccessor, DomainEventsAccessor>();
}