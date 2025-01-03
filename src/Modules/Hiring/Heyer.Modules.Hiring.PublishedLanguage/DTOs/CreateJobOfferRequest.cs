namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record CreateJobOfferRequest(
    string OfferSummary,
    string JobDescription,
    RemoteWork RemoteWork);