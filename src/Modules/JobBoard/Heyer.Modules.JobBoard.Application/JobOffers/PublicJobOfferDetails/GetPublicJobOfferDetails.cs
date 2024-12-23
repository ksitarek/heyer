using FluentResults;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.PublishedLanguage;
using MediatR;

namespace Heyer.Modules.JobBoard.Application.JobOffers.PublicJobOfferDetails;

public record GetPublicJobOfferDetails(Guid Guid) : IRequest<Result<PublishedJobOfferDetails>>, IQuery<PublishedJobOfferDetails>;