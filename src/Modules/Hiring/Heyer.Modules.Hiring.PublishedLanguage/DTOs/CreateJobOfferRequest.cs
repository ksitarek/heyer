namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record CreateJobOfferRequest(
    string OfferSummary,
    string JobDescription,
    RemoteWork RemoteWork);

public record PublishJobOfferRequest(
    Guid JobOfferId,
    DateTimeOffset? PublishedUntil = null);