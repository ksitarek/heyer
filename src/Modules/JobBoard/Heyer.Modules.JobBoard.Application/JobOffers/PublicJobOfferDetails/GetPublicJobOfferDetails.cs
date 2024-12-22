using FluentResults;
using Heyer.API.Client.PublishedLanguage;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using MediatR;

namespace Heyer.Modules.JobBoard.Application.JobOffers.PublicJobOfferDetails;

public record GetPublicJobOfferDetails(Guid Guid) : IRequest<Result<JobOfferDetails>>, IQuery<JobOfferDetails>;