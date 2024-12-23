using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.PublishedLanguage;

namespace Heyer.Modules.JobBoard.Application.JobOffers.PublicJobOfferDetails;

public record GetPublicJobOfferDetails(Guid Guid) : IQuery<PublishedJobOfferDetails>;