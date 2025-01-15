using FluentResults;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.Hiring.Application.JobOffers.List;

public class GetJobListHandler : IRequestHandler<GetJobOffersList, Result<IEnumerable<JobOfferListItem>>>
{
    private readonly IJobOffersRepository _jobOffersRepository;

    public GetJobListHandler(IJobOffersRepository jobOffersRepository) => _jobOffersRepository = jobOffersRepository;

    public async Task<Result<IEnumerable<JobOfferListItem>>> Handle(GetJobOffersList request,
                                                                    CancellationToken cancellationToken)
    {
        try
        {
            var jobOffers = _jobOffersRepository.GetPageQuery()
                .Select(x => new JobOfferListItem(x.Id.Guid, x.OfferSummary, x.PublishedAt, x.PublishedUntil));

            return Result.Ok<IEnumerable<JobOfferListItem>>(await jobOffers.ToListAsync(cancellationToken));
        }
        catch (Exception e)
        {
            return new Result<IEnumerable<JobOfferListItem>>()
                .WithError(new Error("Unable to retrieve job offers").CausedBy(e));
        }
    }
}