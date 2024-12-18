using FluentResults;
using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.JobOffers.Rules;

public class JobOfferMustHaveRequirementsWhenPublishing : IBusinessRule
{
    private readonly Requirements? _requirements;

    public JobOfferMustHaveRequirementsWhenPublishing(Requirements? requirements)
    {
        _requirements = requirements;
    }
    
    public Result Challenge()
    {
        return Result.OkIf(_requirements is not null, "Job offer must have requirements when publishing.");
    }
}