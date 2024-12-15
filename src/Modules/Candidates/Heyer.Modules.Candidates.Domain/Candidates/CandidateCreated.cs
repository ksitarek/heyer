using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Candidates.Domain.Candidates;

public record CandidateCreated(CandidateId CandidateId) : DomainEvent;