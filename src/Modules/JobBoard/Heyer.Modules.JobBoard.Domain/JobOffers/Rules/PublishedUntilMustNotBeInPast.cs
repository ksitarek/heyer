using FluentResults;
using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.JobOffers.Rules;

public class PublishedUntilMustNotBeInPast : IBusinessRule
{
    private readonly DateTimeOffset? _publishedUntil;

    public PublishedUntilMustNotBeInPast(DateTimeOffset? publishedUntil) => _publishedUntil = publishedUntil;

    public Result Challenge() =>
        Result.OkIf(
            _publishedUntil is null || _publishedUntil > DateTimeOffset.UtcNow,
            "Published until date must not be in the past.");
}