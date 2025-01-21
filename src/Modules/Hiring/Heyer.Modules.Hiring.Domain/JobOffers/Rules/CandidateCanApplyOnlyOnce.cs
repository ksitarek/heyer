using FluentResults;
using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.Hiring.Domain.Candidates;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Domain.JobOffers.Rules;

public class CandidateCanApplyOnlyOnce : IBusinessRule
{
    private readonly CandidateId _candidateId;
    private readonly HashSet<CandidateId>? _candidateIds;

    public CandidateCanApplyOnlyOnce(HashSet<CandidateId>? candidateIds, CandidateId candidateId)
    {
        _candidateIds = candidateIds;
        _candidateId = candidateId;
    }

    public Result Challenge() =>
        Result.OkIf(
            _candidateIds is null || !_candidateIds.Contains(_candidateId),
            $"Candidate with id: {_candidateId} has already applied for this job offer.");
}

public class JobOfferMustHaveEmploymentType : IBusinessRule
{
    private readonly List<ContractDetails> _contractsDetails;
    private readonly EmploymentType _employmentType;

    public JobOfferMustHaveEmploymentType(List<ContractDetails> contractsDetails,
                                          EmploymentType employmentType)
    {
        _contractsDetails = contractsDetails;
        _employmentType = employmentType;
    }

    public Result Challenge() =>
        Result.OkIf(_contractsDetails.Any(x => x.EmploymentType == _employmentType),
                    $"Job offer must have contract details for employment type: {_employmentType}.");
}