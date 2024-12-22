using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record JobOfferPublished(PublishedJobOfferId PublishedJobOfferId) : DomainEvent;