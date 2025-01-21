using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;

namespace Heyer.Modules.Hiring.Application.JobOffers.ContractDetails.AddContractDetails;

public record AddContractDetails(JobOfferId Id, PublishedLanguage.DTOs.ContractDetails ContractDetails)
    : ICommand;