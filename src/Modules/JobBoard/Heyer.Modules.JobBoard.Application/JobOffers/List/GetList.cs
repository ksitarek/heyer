using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.JobBoard.PublishedLanguage.DTOs;

namespace Heyer.Modules.JobBoard.Application.JobOffers.List;

public record GetList : IQuery<IEnumerable<PublishedJobOfferListItem>>;