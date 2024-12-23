namespace Heyer.Modules.Hiring.PublishedLanguage;

public record JobOfferDetails(
    Guid Id,
    CompanyDetails CompanyDetails,
    string OfferSummary,
    string JobDescription,
    DateTimeOffset? PublishedAt,
    DateTimeOffset? PublishedUntil,
    OfficeLocation OfficeLocationD,
    RemoteWork RemoteWork,
    Requirements Requirements,
    List<ContractDetails> ContractDetails)
{
}