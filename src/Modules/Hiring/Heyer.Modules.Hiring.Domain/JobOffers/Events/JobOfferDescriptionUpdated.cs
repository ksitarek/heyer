using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.JobOffers.Events;

public record JobOfferDescriptionUpdated(JobOfferId JobOfferId) : DomainEvent;