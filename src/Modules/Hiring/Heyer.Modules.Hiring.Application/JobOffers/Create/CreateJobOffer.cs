using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage;

namespace Heyer.Modules.Hiring.Application.JobOffers.Create;

public record CreateJobOffer(string OfferSummary, string JobDescription, RemoteWork RemoteWork) : ICommand<Guid>;