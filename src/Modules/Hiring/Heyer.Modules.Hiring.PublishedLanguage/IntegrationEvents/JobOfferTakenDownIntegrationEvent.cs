using Heyer.BuildingBlocks.Infrastructure.Integration;

namespace Heyer.Modules.Hiring.PublishedLanguage.IntegrationEvents;

public record JobOfferTakenDownIntegrationEvent : IntegrationEvent
{
    public JobOfferTakenDownIntegrationEvent(Guid id, DateTime occurredOn, Guid jobOfferId) : base(id, occurredOn) =>
        JobOfferId = jobOfferId;

    public Guid JobOfferId { get; set; }
}