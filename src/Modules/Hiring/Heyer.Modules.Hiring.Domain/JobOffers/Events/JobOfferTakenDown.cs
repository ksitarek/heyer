using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.JobOffers.Events;

public record JobOfferTakenDown(JobOfferId JobOfferId) : DomainEvent;