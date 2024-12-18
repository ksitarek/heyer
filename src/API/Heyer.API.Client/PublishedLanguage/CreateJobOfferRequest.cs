namespace Heyer.API.Client.PublishedLanguage;

public record CreateJobOfferRequest(
    string OfferSummary, 
    string JobDescription, 
    RemoteWork RemoteWork);