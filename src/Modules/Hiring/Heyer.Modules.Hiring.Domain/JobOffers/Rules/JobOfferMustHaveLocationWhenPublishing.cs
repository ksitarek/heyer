using FluentResults;
using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.JobOffers.Rules;

public class JobOfferMustHaveLocationWhenPublishing : IBusinessRule
{
    private readonly OfficeLocation? _location;

    public JobOfferMustHaveLocationWhenPublishing(OfficeLocation? location) => _location = location;

    public Result Challenge() => Result.OkIf(_location is not null, "Job offer must have location when publishing.");
}