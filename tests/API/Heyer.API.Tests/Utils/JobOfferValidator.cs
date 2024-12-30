using FluentAssertions;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Heyer.API.Tests.Utils;

public class JobOfferValidator : IDisposable
{
    private readonly Guid _companyId;
    private readonly HiringDbContext _ctx;

    public JobOfferValidator(Guid companyId)
    {
        _companyId = companyId;
        _ctx = GetContext();
    }

    private DbSet<JobOffer> Set => _ctx.Set<JobOffer>();

    public void Dispose() => _ctx.Dispose();

    public async Task ValidateJobOfferIsSavedAsync(Guid id)
    {
        var record = await Set.Where(x => x.Id == new JobOfferId(id)).Select(x => x.Id).FirstOrDefaultAsync();

        record.Should().NotBeNull();
        record!.Guid.Should().Be(id);
    }

    private HiringDbContext GetContext()
    {
        var connectionString =
            ApplicationFactoryConfiguration.InMemoryConfiguration[$"Companies:{_companyId}:SqlServer:ConnectionString"];

        var options = new DbContextOptionsBuilder<HiringDbContext>()
            .UseSqlServer(connectionString)
            .EnableServiceProviderCaching(false)
            .Options;

        return new HiringDbContext(options);
    }
}