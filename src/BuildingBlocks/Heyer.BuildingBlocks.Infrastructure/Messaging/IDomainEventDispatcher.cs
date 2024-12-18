using FluentResults;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public interface IDomainEventDispatcher
{
    Task<Result> DispatchDomainEventsAsync(CancellationToken cancellationToken = default);
}