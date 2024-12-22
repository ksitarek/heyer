using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record JobOfferTakenDown(PublishedJobOfferId PublishedJobOfferId, DateTimeOffset? PublishedUntil) : DomainEvent;