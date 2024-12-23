using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.JobOffers.Events;

public record JobOfferDescriptionUpdated(PublishedJobOfferId PublishedJobOfferId) : DomainEvent;