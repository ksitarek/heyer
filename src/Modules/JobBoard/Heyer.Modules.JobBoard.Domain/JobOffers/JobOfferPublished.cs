using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record JobOfferPublished(JobOfferId JobOfferId) : DomainEvent;