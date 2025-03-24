using FluentResults;
using MediatR;
using Serilog;
using Serilog.Context;

namespace Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;

public class LoggingMiddleware<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : ResultBase, new()
{
    public async Task<TResult> Handle(TRequest request,
                                      RequestHandlerDelegate<TResult> next,
                                      CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("RequestName", typeof(TRequest).Name))
        {
            Log.Information("Handling {RequestName}", typeof(TRequest).Name);

            try
            {
                var result = await next();

                if (result.IsSuccess)
                {
                    Log.Information("Handled {RequestName}", typeof(TRequest).Name);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Log.Error("Error when handling {RequestName}: {Message}",
                                  typeof(TRequest).Name,
                                  error.Message);

                        foreach (var reason in error.Reasons)
                        {
                            Log.Error(reason.ToString()!);
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error when handling {RequestName}", typeof(TRequest).Name);

                var result = new TResult();

                result.Reasons.Add(new ExceptionalError(e.Message, e));

                return result;
            }
        }
    }
}