using FluentResults;
using Heyer.BuildingBlocks.Infrastructure;
using MediatR;

namespace Heyer.Modules.Candidates.Infrastructure.Mediator.Middleware;

public class UnitOfWorkMiddleware<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : ResultBase, new()
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkMiddleware(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var result = await next();

        if(result.IsSuccess)
            await _unitOfWork.CommitAsync(cancellationToken);

        return result;
    }
}