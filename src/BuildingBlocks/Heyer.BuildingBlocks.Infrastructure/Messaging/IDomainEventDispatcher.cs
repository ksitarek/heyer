using FluentResults;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public interface IDomainEventDispatcher
{
    Task<Result> DispatchEventsAsync(CancellationToken cancellationToken = default);
}