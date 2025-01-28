using FluentResults;
using Heyer.BuildingBlocks.Application.HttpLanguage;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.Hiring.Infrastructure.Persistence;

internal class JobOffersRepository : IJobOffersRepository
{
    private readonly HiringDbContext _context;

    public JobOffersRepository(HiringDbContext context) => _context = context;

    public async Task<Result> AddAsync(JobOffer jobOffer,
                                       CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.JobOffers.AddAsync(jobOffer, cancellationToken);

            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(new Error("Could not add job offer").CausedBy(e));
        }
    }

    public Task<bool> CheckForConflicts(JobOffer subject,
                                        CancellationToken cancellationToken = default) =>
        _context.JobOffers
            .Where(x => x.Id != subject.Id)
            .Where(x => x.OfferSummary == subject.OfferSummary)
            .Where(x => x.PublishedAt != null && x.PublishedAt <= DateTime.UtcNow &&
                        (x.PublishedUntil == null || x.PublishedUntil >= DateTime.UtcNow))
            .AnyAsync(cancellationToken);

    public Task<JobOffer?> GetJobOfferById(JobOfferId jobOfferId,
                                           CancellationToken cancellationToken = default) =>
        _context.JobOffers
            .Where(x => x.Id == jobOfferId)
            .FirstOrDefaultAsync(cancellationToken);

    public IQueryable<JobOffer> GetPageQuery(FilteredListRequest filteredListRequest) =>
        _context.JobOffers
            .OrderBy(x => x.OfferSummary)
            .Skip(filteredListRequest.PageIx * filteredListRequest.PageSize)
            .Take(filteredListRequest.PageSize); // todo implement some sensible sorting/filtering

    public Task<long> GetTotalCount(FilteredListRequest filteredListRequest,
                                    CancellationToken cancellationToken = default) =>
        _context.JobOffers
            .LongCountAsync(cancellationToken);
}