using FluentAssertions;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.Infrastructure;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.API.Tests.Utils;

public class JobOfferValidator : IDisposable
{
    private readonly DbSet<PublishedJobOffer> _set;
    private readonly IServiceScope _scope;

    public JobOfferValidator()
    {
        _scope = JobBoardModuleCompositionRoot.CreateScope();

        var dbContext = _scope.ServiceProvider.GetRequiredService<JobBoardContext>();

        _set = dbContext.Set<PublishedJobOffer>();
    }

    public async Task ValidateJobOfferIsSavedAsync(Guid id)
    {
        var record = await _set.Where(x => x.Id == new PublishedJobOfferId(id)).Select(x => x.Id).FirstOrDefaultAsync();

        record.Should().NotBeNull();
        record!.Guid.Should().Be(id);
    }

    public void Dispose() => _scope.Dispose();
}