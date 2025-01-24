namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record UpdateJobOfferRequest(
    Guid JobOfferId,
    string OfferSummary,
    string JobDescription,
    RemoteWork RemoteWork);