using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.JobOffers;

public record JobOfferTakenDown(JobOfferId JobOfferId) : DomainEvent;