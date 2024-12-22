using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.JobOffers;

public record JobOfferPublished(JobOfferId JobOfferId) : DomainEvent;