using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Application.JobOffers.List;

public record GetJobOffersList : IQuery<IEnumerable<JobOfferListItem>>;