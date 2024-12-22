using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.JobOffers;

public record JobOfferCreated(JobOfferId JobOfferId) : DomainEvent;