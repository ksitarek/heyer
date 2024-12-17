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

public class PublishedUntilMustNotBeInPast : IBusinessRule
{
    private readonly DateTimeOffset? _publishedUntil;

    public PublishedUntilMustNotBeInPast(DateTimeOffset? publishedUntil)
    {
        _publishedUntil = publishedUntil;
    }
    
    public Result Challenge()
    {
        return Result.OkIf(
            _publishedUntil is null || _publishedUntil > DateTimeOffset.UtcNow,
            "Published until date must not be in the past.");
    }
}