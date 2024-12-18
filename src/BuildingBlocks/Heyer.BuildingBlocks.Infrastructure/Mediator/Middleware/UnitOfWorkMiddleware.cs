using FluentResults;
using MediatR;

namespace Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;

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
        try
        {
            var handleResult = await next();

            if (handleResult.IsSuccess)
            {
                var uowResult = await _unitOfWork.CommitAsync(cancellationToken);
                if(uowResult.IsFailed)
                {
                    handleResult.Reasons.AddRange(uowResult.Errors);
                }
            }

            return handleResult;
        }
        catch (Exception e)
        {
            var result = new TResult();
            result.Errors.Add(new Error(e.Message).CausedBy(e));

            return result;
        }
    }
}