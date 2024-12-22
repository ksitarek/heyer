using FluentResults;
using Heyer.BuildingBlocks.Infrastructure.Messaging;

namespace Heyer.BuildingBlocks.Infrastructure.Modules;

public interface IModuleRunner
{
    Task<Result> DispatchCommand<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand;

    Task<Result<TResult>> DispatchCommand<TCommand, TResult>(TCommand query, CancellationToken cancellationToken)
        where TCommand : ICommand<TResult>;

    Task<Result<TResult>> DispatchQuery<TQuery, TResult>(TQuery query, CancellationToken cancellationToken)
        where TQuery : IQuery<TResult>;
}