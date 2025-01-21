using FluentResults;

namespace Heyer.BuildingBlocks.Application.Results;

public class BusinessRuleViolation : Error
{
    public BusinessRuleViolation() : base("Business rule violation.")
    {
    }
}