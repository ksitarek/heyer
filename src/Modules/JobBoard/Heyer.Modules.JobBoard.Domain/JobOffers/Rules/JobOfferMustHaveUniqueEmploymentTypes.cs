using FluentResults;
using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.JobOffers.Rules;

public class JobOfferMustHaveUniqueEmploymentTypes : IBusinessRule
{
    private readonly Dictionary<EmploymentType, ContractDetails>? _contractsDetails;
    private readonly EmploymentType _employmentType;

    public JobOfferMustHaveUniqueEmploymentTypes(Dictionary<EmploymentType, ContractDetails>? contractsDetails,
                                                 EmploymentType employmentType)
    {
        _contractsDetails = contractsDetails;
        _employmentType = employmentType;
    }

    public Result Challenge() =>
        _contractsDetails is null
            ? Result.Ok()
            : Result.OkIf(
                !_contractsDetails.ContainsKey(_employmentType),
                $"Job offer already has contract details for employment type: {_employmentType}.");
}