using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.JobBoard.PublishedLanguage.DTOs;

public record PublishedJobOfferListItem(
    Guid Id,
    string OfferSummary,
    RemoteWork RemoteWork,
    List<ContractDetails> ContractsDetails,
    string LocationCity,
    string LocationCountry,
    string CompanyName,
    DateTimeOffset PublishedAt);