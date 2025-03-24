using FluentResults;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.BuildingBlocks.Infrastructure.Modules;

public abstract class ModuleRunner : IModuleRunner
{
    protected ModuleRunner(Func<IServiceScope> scopeProvider) => ScopeProvider = scopeProvider;

    private Func<IServiceScope> ScopeProvider { get; init; }

    public async Task<Result> DispatchCommand<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand
    {
        using var scope = ScopeProvider.Invoke();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<Result<TResult>> DispatchCommand<TCommand, TResult>(TCommand command,
                                                                          CancellationToken cancellationToken)
        where TCommand : ICommand<TResult>
    {
        using var scope = ScopeProvider.Invoke();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<Result<TResult>> DispatchQuery<TQuery, TResult>(TQuery query, CancellationToken cancellationToken)
        where TQuery : IQuery<TResult>
    {
        using var scope = ScopeProvider.Invoke();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(query, cancellationToken);
    }
}