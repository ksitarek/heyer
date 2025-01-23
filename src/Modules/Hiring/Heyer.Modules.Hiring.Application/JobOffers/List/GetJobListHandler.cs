using FluentResults;
using Heyer.BuildingBlocks.Application.HttpLanguage;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.Hiring.Application.JobOffers.List;

public class GetJobListHandler : IRequestHandler<GetJobOffersList, Result<ListResponse<JobOfferListItem>>>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public GetJobListHandler(IJobOffersRepository jobOffersRepository) => _jobOffersRepository = jobOffersRepository;

    public async Task<Result<ListResponse<JobOfferListItem>>> Handle(GetJobOffersList request,
                                                                     CancellationToken cancellationToken)
    {
        try
        {
            var pagedQuery = _jobOffersRepository.GetPageQuery(request);

            var totalCount = await _jobOffersRepository.GetTotalCount(request, cancellationToken);

            var jobOffers = pagedQuery
                .Select(x => new JobOfferListItem(x.Id.Guid, x.OfferSummary, x.PublishedAt, x.PublishedUntil));

            return Result.Ok(ListResponse<JobOfferListItem>.Create(
                                 await jobOffers.ToListAsync(cancellationToken),
                                 request.PageSize,
                                 totalCount));
        }
        catch (Exception e)
        {
            return new Result<ListResponse<JobOfferListItem>>()
                .WithError(new Error("Unable to retrieve job offers").CausedBy(e));
        }
    }
}