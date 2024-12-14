using FluentResults;

namespace Heyer.BuildingBlocks.Infrastructure;

public interface IUnitOfWork
{
    Task<Result<int>> CommitAsync(CancellationToken cancellationToken);
}