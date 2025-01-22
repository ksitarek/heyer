using FluentResults;
using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Domain.JobOffers.Rules;

public class JobOfferMustHaveEmploymentType : IBusinessRule
{
    private readonly List<ContractDetails>? _contractsDetails;
    private readonly EmploymentType _employmentType;

    public JobOfferMustHaveEmploymentType(List<ContractDetails>? contractsDetails,
                                          EmploymentType employmentType)
    {
        _contractsDetails = contractsDetails;
        _employmentType = employmentType;
    }

    public Result Challenge() =>
        Result.OkIf(_contractsDetails != null && _contractsDetails.Any(x => x.EmploymentType == _employmentType),
                    $"Job offer must have contract details for employment type: {_employmentType}.");
}