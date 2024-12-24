using FluentAssertions;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

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

    private DbSet<JobOffer> _set => _ctx.Set<JobOffer>();

    public void Dispose() => _ctx.Dispose();

    public async Task ValidateJobOfferIsSavedAsync(Guid id)
    {
        var record = await _set.Where(x => x.Id == new JobOfferId(id)).Select(x => x.Id).FirstOrDefaultAsync();

        record.Should().NotBeNull();
        record!.Guid.Should().Be(id);
    }

    private HiringDbContext GetContext()
    {
        var connectionString =
            ApplicationFactoryConfiguration.InMemoryConfiguration[$"Companies:{_companyId}:MongoDb:ConnectionString"];
        var databaseName =
            ApplicationFactoryConfiguration.InMemoryConfiguration[$"Companies:{_companyId}:MongoDb:DatabaseName"];

        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(databaseName);

        var options = new DbContextOptionsBuilder<HiringDbContext>()
            .UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName)
            .EnableServiceProviderCaching(false)
            .Options;

        return new HiringDbContext(options);
    }
}