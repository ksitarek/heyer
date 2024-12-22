using FluentResults;
using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.JobOffers.Rules;

public class JobOfferMustBePublishedToTakeDown : IBusinessRule
{
    private readonly DateTimeOffset? _publishedAt;

    public JobOfferMustBePublishedToTakeDown(DateTimeOffset? publishedAt) => _publishedAt = publishedAt;

    public Result Challenge() => Result.OkIf(_publishedAt is not null, "Job offer must be published to take it down.");
}