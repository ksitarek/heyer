using FluentResults;
using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.JobBoard.Domain.Candidates;

namespace Heyer.Modules.JobBoard.Domain.JobOffers.Rules;

public class CandidateCanApplyOnlyOnce : IBusinessRule
{
    private readonly HashSet<CandidateId>? _candidateIds;
    private readonly CandidateId _candidateId;

    public CandidateCanApplyOnlyOnce(HashSet<CandidateId>? candidateIds, CandidateId candidateId)
    {
        _candidateIds = candidateIds;
        _candidateId = candidateId;
    }
    
    public Result Challenge()
    {
        return Result.OkIf(
            _candidateIds is null || !_candidateIds.Contains(_candidateId),
            $"Candidate with id: {_candidateId} has already applied for this job offer.");
    }
}