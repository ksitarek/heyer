using FluentAssertions;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;

namespace Heyer.API.Tests.Utils;

public class JobOfferValidator
{
    private readonly DbSet<JobOffer> _set;

    public JobOfferValidator(DbContext dbContext) => _set = dbContext.Set<JobOffer>();

    public async Task ValidateJobOfferIsSavedAsync(Guid id)
    {
        var record = await _set.Where(x => x.Id == new JobOfferId(id)).FirstOrDefaultAsync();

        record.Should().NotBeNull();
        record!.Id.Guid.Should().Be(id);
    }
}