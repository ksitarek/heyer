using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record JobOfferCreated(JobOfferId JobOfferId) : DomainEvent;