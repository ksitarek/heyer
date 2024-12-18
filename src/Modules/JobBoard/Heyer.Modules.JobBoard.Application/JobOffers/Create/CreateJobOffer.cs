using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.JobBoard.Domain.JobOffers;

namespace Heyer.Modules.JobBoard.Application.JobOffers.Create;

public record CreateJobOffer(CompanyDetails CompanyDetails, string OfferSummary, string JobDescription, RemoteWork RemoteWork) : ICommand;