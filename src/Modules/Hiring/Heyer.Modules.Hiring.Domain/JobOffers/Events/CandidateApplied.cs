using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.Hiring.Domain.Candidates;

namespace Heyer.Modules.Hiring.Domain.JobOffers.Events;

public record CandidateApplied(JobOfferId JobOfferId, CandidateId CandidateId) : DomainEvent;