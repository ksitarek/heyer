using FluentResults;
using MediatR;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public interface IQuery<TResult> : IRequest<Result<TResult>>
{
}