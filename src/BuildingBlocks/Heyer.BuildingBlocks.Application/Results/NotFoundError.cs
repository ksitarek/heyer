using FluentResults;

namespace Heyer.BuildingBlocks.Application.Results;

public class NotFoundError : Error
{
    public NotFoundError() : base("Not found.")
    {
    }
}