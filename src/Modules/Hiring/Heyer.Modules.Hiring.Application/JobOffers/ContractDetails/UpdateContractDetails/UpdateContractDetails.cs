using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Application.JobOffers.ContractDetails.UpdateContractDetails;

public record UpdateContractDetails(
    JobOfferId Id,
    EmploymentType EmploymentType,
    SalaryRange SalaryRange,
    int TimeNumerator,
    int TimeDenominator)
    : ICommand;