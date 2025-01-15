namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record JobOfferListItem(Guid Id, string OfferSummary, DateTimeOffset? PublishedAt, DateTimeOffset? PublishedUntil)
{
}