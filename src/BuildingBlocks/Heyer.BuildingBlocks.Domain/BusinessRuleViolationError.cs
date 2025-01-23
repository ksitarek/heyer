using FluentResults;

namespace Heyer.BuildingBlocks.Application.Results;

public class BusinessRuleViolationError : Error
{
    public BusinessRuleViolationError() : base("Business rule violation.")
    {
    }
}