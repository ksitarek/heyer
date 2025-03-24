using FluentResults;

namespace Heyer.BuildingBlocks.Domain;

public class BusinessRuleViolationError : Error
{
    public BusinessRuleViolationError() : base("Business rule violation.")
    {
    }
}