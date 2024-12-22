using FluentResults;
using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.JobOffers.Rules;

public class JobOfferMustHaveUniqueEmploymentTypes : IBusinessRule
{
    private readonly ICollection<ContractDetails>? _contractsDetails;
    private readonly EmploymentType _employmentType;

    public JobOfferMustHaveUniqueEmploymentTypes(ICollection<ContractDetails>? contractsDetails,
                                                 EmploymentType employmentType)
    {
        _contractsDetails = contractsDetails;
        _employmentType = employmentType;
    }

    public Result Challenge() =>
        _contractsDetails is null
            ? Result.Ok()
            : Result.OkIf(_contractsDetails.All(cd => cd.EmploymentType != _employmentType),
                          $"Job offer already has contract details for employment type: {_employmentType}.");
}