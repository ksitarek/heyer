using Heyer.BuildingBlocks.Infrastructure.Integration.Processing;

namespace Heyer.BuildingBlocks.Infrastructure.Integration;

public class InProcessEventBus : IEventBus
{
    private readonly Dictionary<string, List<IIntegrationEventHandler>> _handlers = new();

    public Task Publish<T>(T @event) where T : IntegrationEvent
    {
        var eventTypeName = @event.GetType().Name;
        if (_handlers.TryGetValue(eventTypeName, out var handlers))
        {
            var tasks = handlers.Select(x => ((IIntegrationEventHandler<T>)x).Handle(@event));

            return Task.WhenAll(tasks);
        }

        return Task.CompletedTask;
    }

    public Task Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
    {
        var eventTypeName = typeof(T).Name;
        if (_handlers.TryGetValue(eventTypeName, out var handlers))
        {
            handlers.Add(handler);
        }
        else

        {
            _handlers.Add(eventTypeName, [handler]);
        }

        return Task.CompletedTask;
    }
}