namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync();
}