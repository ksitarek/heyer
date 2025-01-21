namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record AddContractDetailsRequest(
    Guid JobOfferId,
    ContractDetails ContractDetails);