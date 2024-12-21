using FluentResults;
using Heyer.API.Client.PublishedLanguage;
using MediatR;

namespace Heyer.Modules.JobBoard.Application.JobOffers.PublicJobOfferDetails;

public record GetPublicJobOfferDetails(Guid Guid) : IRequest<Result<JobOfferDetails>>;