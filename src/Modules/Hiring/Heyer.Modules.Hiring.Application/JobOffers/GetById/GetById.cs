using FluentResults;
using Heyer.Modules.Hiring.PublishedLanguage;
using MediatR;

namespace Heyer.Modules.Hiring.Application.JobOffers.GetById;

public record GetById(Guid Guid) : IRequest<Result<PublishedJobOfferDetails>>;