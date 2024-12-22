using FluentResults;
using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.JobBoard.Domain.Candidates;

namespace Heyer.Modules.JobBoard.Domain.JobOffers.Rules;

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