using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.Candidates;

public record CandidateCreated(CandidateId Id) : DomainEvent;