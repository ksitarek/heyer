using FluentResults;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Application.Notifications;
using Heyer.BuildingBlocks.Domain;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Json;
using MediatR;
using Serilog;
using ExecutionContext = Heyer.BuildingBlocks.Application.Authorization.ExecutionContext;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

internal class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IDomainEventNotificationsRegistry _domainEventNotificationsRegistry;
    private readonly IDomainEventsAccessor _domainEventsAccessor;
    private readonly IMediator _mediator;
    private readonly IOutboxStore _outboxStore;
    private readonly IUserDataProvider _userDataProvider;

    public DomainEventDispatcher(
        IMediator mediator,
        IDomainEventsAccessor domainEventsAccessor,
        IDomainEventNotificationsRegistry domainEventNotificationsRegistry,
        IOutboxStore outboxStore,
        IUserDataProvider userDataProvider)
    {
        _mediator = mediator;
        _domainEventsAccessor = domainEventsAccessor;
        _domainEventNotificationsRegistry = domainEventNotificationsRegistry;
        _outboxStore = outboxStore;
        _userDataProvider = userDataProvider;
    }

    public async Task<Result> DispatchDomainEventsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var domainEvents = _domainEventsAccessor.GetAllDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);

                await ProcessDomainEventNotification(domainEvent);
            }

            _domainEventsAccessor.ClearAllDomainEvents();
        }
        catch (Exception e)
        {
            return Result.Fail(new Error("Failed to dispatch domain events.").CausedBy(e));
        }

        return Result.Ok();
    }

    private Task ProcessDomainEventNotification(DomainEvent domainEvent)
    {
        var domainEventName = domainEvent.GetType().FullName!;

        if (_domainEventNotificationsRegistry.Contains(domainEventName))
        {
            var notificationType = _domainEventNotificationsRegistry.GetNotificationType(domainEventName);

            var domainEventNotification = Activator.CreateInstance(notificationType,
                                                                   domainEvent.EventId,
                                                                   domainEvent,
                                                                   new ExecutionContext(
                                                                       _userDataProvider.UserId,
                                                                       _userDataProvider.CompanyId,
                                                                       _userDataProvider.CompanyName));

            if (domainEventNotification == null)
            {
                Log.Fatal("Could not instantiate domain event notification {DomainEventNotificationType}.",
                          notificationType.Name);

                throw new Exception(
                    $"Could not instantiate domain event notification {notificationType.Name}.");
            }

            return _outboxStore.Store(new OutboxMessage
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Type = notificationType.FullName!,
                Data = domainEventNotification.Serialize()
            });
        }

        return Task.CompletedTask;
    }
}