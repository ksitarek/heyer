using System.Text.Json;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Processing;

public abstract class GenericEventHandler<T> : IIntegrationEventHandler<T> where T : IntegrationEvent
{
    private readonly IInboxStore _inboxStore;
    protected GenericEventHandler(IInboxStore inboxStore) => _inboxStore = inboxStore;

    public Task Handle(T @event) =>
        _inboxStore.Store(new InboxMessage
        {
            Id = Guid.NewGuid(),
            Type = @event.GetType().FullName!,
            Data = JsonSerializer.Serialize(@event),
            CreatedAt = @event.OccurredOn
        });
}