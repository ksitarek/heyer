using FluentResults;
using FluentValidation;
using Heyer.BuildingBlocks.Application.Results;
using MediatR;

namespace Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;

public class ValidationMiddleware<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : ResultBase, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationMiddleware(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next,
                                      CancellationToken cancellationToken)
    {
        var validationResult = await ValidateAsync(request, cancellationToken);
        
        return validationResult.IsSuccess
            ? await next()
            : BuildFailResult(validationResult);
    }

    private static TResult BuildFailResult(Result validationResult)
    {
        var result = new TResult();
        
        foreach (var reason in validationResult.Reasons)
            result.Reasons.Add(reason);

        return result;
    }

    private async Task<Result> ValidateAsync(TRequest request, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return Result.Ok();

        var context = new ValidationContext<TRequest>(request);
        var validationTasks = _validators.Select(x => x.ValidateAsync(context, cancellationToken));
        var validationResults = await Task.WhenAll(validationTasks);

        var failures = validationResults
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .Where(r => r != null)
            .ToList();

        if (failures.Count > 0)
        {
            var validationFailedResult = Result.Fail(new ValidationError(failures));
            return validationFailedResult;
        }

        return Result.Ok();
    }
}