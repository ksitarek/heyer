using FluentResults;
using MediatR;

namespace Heyer.Storage.API.Middleware;

public class MediatorLoggingMiddleware<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : ResultBase, new()
{
    private readonly ILogger<MediatorLoggingMiddleware<TRequest, TResult>> _logger;

    public MediatorLoggingMiddleware(ILogger<MediatorLoggingMiddleware<TRequest, TResult>> logger)
    {
        _logger = logger;
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next,
                                      CancellationToken cancellationToken)
    {
        using (_logger.BeginScope($"Handling {typeof(TRequest).Name}"))
        {
            var result = await next();

            _logger.LogInformation($"Handled {typeof(TRequest).Name}");

            return result;
        }
    }
}