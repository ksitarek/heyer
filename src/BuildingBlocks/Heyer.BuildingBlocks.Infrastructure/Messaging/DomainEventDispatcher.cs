using FluentResults;
using MediatR;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

internal class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly IDomainEventsAccessor _domainEventsAccessor;

    public DomainEventDispatcher(
        IMediator mediator, 
        IDomainEventsAccessor domainEventsAccessor)
    {
        _mediator = mediator;
        _domainEventsAccessor = domainEventsAccessor;
    }
    
    public async Task<Result> DispatchDomainEventsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var domainEvents = _domainEventsAccessor.GetAllDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            _domainEventsAccessor.ClearAllDomainEvents();
        }
        catch (Exception e)
        {
            return Result.Fail(new Error("Failed to dispatch domain events.").CausedBy(e));
        }

        return Result.Ok();
    }
}