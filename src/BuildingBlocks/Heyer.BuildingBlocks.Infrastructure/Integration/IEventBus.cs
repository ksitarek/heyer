using Heyer.BuildingBlocks.Infrastructure.Integration.Processing;

namespace Heyer.BuildingBlocks.Infrastructure.Integration;

public interface IEventBus
{
    Task Publish<T>(T @event)
        where T : IntegrationEvent;

    Task Subscribe<T>(IIntegrationEventHandler<T> handler)
        where T : IntegrationEvent;
}