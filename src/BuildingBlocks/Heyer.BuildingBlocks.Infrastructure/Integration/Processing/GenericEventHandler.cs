using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Processing;

public sealed class GenericEventHandler<T> : IIntegrationEventHandler<T> where T : IntegrationEvent
{
    private readonly IServiceProvider _serviceProvider;

    public GenericEventHandler(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task Handle(T @event)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        if (@event is IntegrationEventWithContext clientIntegrationEvent)
        {
            var userDataProvider =
                scope.ServiceProvider.GetRequiredService<IUserDataProvider>() as ValueUserDataProvider;

            userDataProvider!.SetExecutionContext(
                clientIntegrationEvent.ExecutionContext.UserId,
                clientIntegrationEvent.ExecutionContext.CompanyId,
                clientIntegrationEvent.ExecutionContext.CompanyName);
        }

        var inboxStore = scope.ServiceProvider.GetRequiredService<IInboxStore>();

        await inboxStore.Store(new InboxMessage
        {
            Id = Guid.CreateVersion7(),
            Type = @event.GetType().FullName!,
            Data = @event.Serialize(),
            CreatedAt = @event.OccurredOn
        });
    }
}