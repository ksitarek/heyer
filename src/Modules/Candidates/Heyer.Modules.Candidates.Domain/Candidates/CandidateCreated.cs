using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Candidates.Domain.Candidates;

internal record CandidateCreated(CandidateId CandidateId) : DomainEvent;