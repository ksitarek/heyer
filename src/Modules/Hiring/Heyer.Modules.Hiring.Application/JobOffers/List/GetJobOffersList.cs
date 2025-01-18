using Heyer.BuildingBlocks.Application.HttpLanguage;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Application.JobOffers.List;

public record GetJobOffersList(int Page = 1, int PageSize = 10)
    : FilteredListRequest(Page, PageSize), IQuery<ListResponse<JobOfferListItem>>;