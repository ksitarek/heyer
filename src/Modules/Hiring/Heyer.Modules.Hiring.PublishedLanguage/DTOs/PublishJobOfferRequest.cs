namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record PublishJobOfferRequest(
    Guid JobOfferId,
    DateTimeOffset? PublishedUntil = null);