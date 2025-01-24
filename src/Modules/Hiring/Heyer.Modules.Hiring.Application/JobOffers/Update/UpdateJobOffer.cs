using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Application.JobOffers.Update;

public record UpdateJobOffer(JobOfferId Id, string OfferSummary, string JobDescription, RemoteWork RemoteWork)
    : ICommand;