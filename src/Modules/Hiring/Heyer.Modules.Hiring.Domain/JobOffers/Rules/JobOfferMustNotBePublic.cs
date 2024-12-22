using FluentResults;
using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.JobOffers.Rules;

public class JobOfferMustNotBePublic : IBusinessRule
{
    private readonly DateTimeOffset? _publishedAt;
    private readonly DateTimeOffset? _publishedUntil;

    public JobOfferMustNotBePublic(DateTimeOffset? publishedAt, DateTimeOffset? publishedUntil)
    {
        _publishedAt = publishedAt;
        _publishedUntil = publishedUntil;
    }

    private bool IsPublished =>
        _publishedAt != null && (_publishedUntil == null || _publishedUntil > DateTimeOffset.UtcNow);

    public Result Challenge() =>
        Result.OkIf(
            !IsPublished,
            "Job offer must not be public.");
}