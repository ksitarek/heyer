using System.Reflection;
using Heyer.BuildingBlocks.Domain;

namespace Heyer.BuildingBlocks.Application.Notifications;

public class DomainEventNotificationsRegistry : IDomainEventNotificationsRegistry
{
    private readonly Dictionary<string, Type> _notificationsMap = new();

    public void Add<TNotification, TDomainEvent>()
        where TNotification : IDomainEventNotification<TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        var name = typeof(TDomainEvent).FullName!;

        if (_notificationsMap.ContainsKey(name))
        {
            throw new InvalidOperationException($"Notification with name {name} already exists.");
        }

        _notificationsMap.Add(name, typeof(TNotification));
    }

    public bool Contains(string name) => _notificationsMap.ContainsKey(name);

    public Type GetNotificationType(string name) => _notificationsMap[name];

    public void LoadFromAssembly(Assembly assembly)
    {
        var interfaceType = typeof(IDomainEventNotification<>);

        var interfacePredicate =
            new Func<Type, bool>(t => t.IsInterface && t.IsGenericType &&
                                      t.GetGenericTypeDefinition() == interfaceType);

        var domainNotifications = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(interfacePredicate));

        foreach (var notificationType in domainNotifications)
        {
            var domainEventType = notificationType.GetInterfaces()
                .First(interfacePredicate).GetGenericArguments().First();

            var domainEventTypeName = domainEventType.FullName!;

            if (_notificationsMap.ContainsKey(domainEventTypeName))
            {
                throw new InvalidOperationException($"Notification with name {domainEventTypeName} already exists.");
            }

            _notificationsMap.Add(domainEventType.FullName!, notificationType);
        }
    }
}