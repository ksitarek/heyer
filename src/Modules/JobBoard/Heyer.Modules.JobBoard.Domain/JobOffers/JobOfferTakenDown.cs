using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record JobOfferTakenDown(JobOfferId JobOfferId) : DomainEvent;