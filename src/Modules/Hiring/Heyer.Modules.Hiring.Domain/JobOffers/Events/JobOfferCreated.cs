using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.JobOffers.Events;

public record JobOfferCreated(JobOfferId JobOfferId) : DomainEvent;