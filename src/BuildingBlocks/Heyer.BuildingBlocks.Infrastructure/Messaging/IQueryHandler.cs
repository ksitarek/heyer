using FluentResults;
using MediatR;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, Result<TResult>>
    where TQuery : IQuery<TResult>, IRequest<Result<TResult>>
{
}