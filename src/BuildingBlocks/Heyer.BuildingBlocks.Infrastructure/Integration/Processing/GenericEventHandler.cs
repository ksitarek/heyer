using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Json;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Processing;

public sealed class GenericEventHandler<T> : IIntegrationEventHandler<T> where T : IntegrationEvent
{
    private readonly IInboxStore _inboxStore;
    public GenericEventHandler(IInboxStore inboxStore) => _inboxStore = inboxStore;

    public Task Handle(T @event) =>
        _inboxStore.Store(new InboxMessage
        {
            Id = Guid.CreateVersion7(),
            Type = @event.GetType().FullName!,
            Data = @event.Serialize(),
            CreatedAt = @event.OccurredOn
        });
}