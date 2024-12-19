using MediatR;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : INotification
{
}