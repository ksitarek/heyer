using Heyer.BuildingBlocks.Infrastructure.Messaging;

namespace Heyer.Modules.Hiring.Application.JobOffers.Publish;

public record PublishJobOffer(Guid JobOfferId, DateTimeOffset? PublishUntil) : ICommand;