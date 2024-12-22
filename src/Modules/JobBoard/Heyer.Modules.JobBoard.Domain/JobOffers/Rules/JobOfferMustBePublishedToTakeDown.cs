using FluentResults;
using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.JobOffers.Rules;

public class JobOfferMustBePublishedToTakeDown : IBusinessRule
{
    private readonly DateTimeOffset? _publishedUntil;

    public JobOfferMustBePublishedToTakeDown(DateTimeOffset? publishedUntil) => _publishedUntil = publishedUntil;

    public Result Challenge() => Result.OkIf(_publishedUntil is null || _publishedUntil >= DateTimeOffset.UtcNow,
                                             "Job offer must be published to take it down.");
}