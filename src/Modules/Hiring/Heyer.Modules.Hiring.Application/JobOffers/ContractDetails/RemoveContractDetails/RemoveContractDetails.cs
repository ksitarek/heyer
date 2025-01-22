using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Application.JobOffers.ContractDetails.RemoveContractDetails;

public record RemoveContractDetails(JobOfferId Id, EmploymentType EmploymentType) : ICommand;