namespace Heyer.Modules.Hiring.PublishedLanguage;

public record CreateJobOfferRequest(
    string OfferSummary,
    string JobDescription,
    RemoteWork RemoteWork);