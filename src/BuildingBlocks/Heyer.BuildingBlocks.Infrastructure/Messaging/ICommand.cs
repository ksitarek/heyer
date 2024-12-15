using FluentResults;
using MediatR;

namespace Heyer.BuildingBlocks.Infrastructure.Messaging;

public interface ICommand : IRequest<Result>
{
    
}