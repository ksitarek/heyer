using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.JobBoard.Domain.Candidates;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record CandidateApplied(JobOfferId JobOfferId, CandidateId CandidateId) : DomainEvent;