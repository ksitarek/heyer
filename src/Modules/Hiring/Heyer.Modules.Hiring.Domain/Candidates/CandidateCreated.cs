using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.Candidates;

public record CandidateCreated(CandidateId Id) : DomainEvent;