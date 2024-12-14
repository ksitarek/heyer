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
    
    public Task DispatchEventsAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = _domainEventsAccessor.GetAllDomainEvents();
        
        foreach (var domainEvent in domainEvents)
        {
            _mediator.Publish(domainEvent);
        }
        
        _domainEventsAccessor.ClearAllDomainEvents();
        
        return Task.CompletedTask;
    }
}