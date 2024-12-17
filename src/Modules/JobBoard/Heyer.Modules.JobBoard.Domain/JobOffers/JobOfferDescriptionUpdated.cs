using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record JobOfferDescriptionUpdated(JobOfferId JobOfferId) : DomainEvent;