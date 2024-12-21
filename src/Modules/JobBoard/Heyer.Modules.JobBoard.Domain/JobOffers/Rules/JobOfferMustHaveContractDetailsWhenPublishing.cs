using FluentResults;
using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.JobOffers.Rules;

public class JobOfferMustHaveContractDetailsWhenPublishing : IBusinessRule
{
    private readonly List<ContractDetails>? _contractsDetails;

    public JobOfferMustHaveContractDetailsWhenPublishing(List<ContractDetails>? contractsDetails) =>
        _contractsDetails = contractsDetails;

    public Result Challenge() =>
        Result.OkIf(_contractsDetails != null && _contractsDetails.Any(),
                    "Job offer must have at least one contract details when publishing.");
}