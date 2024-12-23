using FluentResults;
using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.Hiring.PublishedLanguage;

namespace Heyer.Modules.Hiring.Domain.JobOffers.Rules;

public class JobOfferMustHaveRequirementsWhenPublishing : IBusinessRule
{
    private readonly Requirements? _requirements;

    public JobOfferMustHaveRequirementsWhenPublishing(Requirements? requirements) => _requirements = requirements;

    public Result Challenge() =>
        Result.OkIf(_requirements is not null, "Job offer must have requirements when publishing.");
}