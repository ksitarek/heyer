using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.JobOffers;

public record JobOfferDescriptionUpdated(JobOfferId JobOfferId) : DomainEvent;