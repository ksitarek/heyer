using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;

public class LoggingMiddleware<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : ResultBase, new()
{
    private readonly ILogger<LoggingMiddleware<TRequest, TResult>> _logger;

    public LoggingMiddleware(ILogger<LoggingMiddleware<TRequest, TResult>> logger)
    {
        _logger = logger;
    }

    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next,
                                      CancellationToken cancellationToken)
    {
        using (_logger.BeginScope("Handling {RequestName}", typeof(TRequest).Name))
        {
            var result = await next();

            if (result.IsSuccess)
            {
                _logger.LogInformation("Handled {RequestName}", typeof(TRequest).Name);
            }
            else
            {
                _logger.LogError("Error when handling {RequestName}: {Message}",
                                 typeof(TRequest).Name,
                                 string.Join(", ",
                                             result.Errors.Select(e => e.Message)));
            }

            return result;
        }
    }
}