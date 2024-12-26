namespace Heyer.BuildingBlocks.Infrastructure.Integration.Processing;

public interface IIntegrationEventHandler<T> : IIntegrationEventHandler where T : IntegrationEvent
{
    Task Handle(T @event);
}