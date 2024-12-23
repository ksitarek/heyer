using FluentAssertions;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Infrastructure;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.Infrastructure;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Heyer.API.Tests.Utils;

public class JobOfferValidator : IDisposable
{
    private readonly Guid _companyId;
    private DbSet<JobOffer> _set => _ctx.Set<JobOffer>();
    private readonly HiringDbContext _ctx;

    public JobOfferValidator(Guid companyId)
    {
        _companyId = companyId;
        _ctx = GetContext();
    }

    private HiringDbContext GetContext()
    {
        var connectionString = ApplicationFactoryConfiguration.InMemoryConfiguration[$"Companies:{_companyId}:MongoDb:ConnectionString"];
        var databaseName = ApplicationFactoryConfiguration.InMemoryConfiguration[$"Companies:{_companyId}:MongoDb:DatabaseName"];

        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(databaseName);

        var options = new DbContextOptionsBuilder<HiringDbContext>()
            .UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName)
            .Options;

        return new HiringDbContext(options);
    }

    public void Dispose() => _ctx.Dispose();

    public async Task ValidateJobOfferIsSavedAsync(Guid id)
    {
        var record = await _set.Where(x => x.Id == new JobOfferId(id)).Select(x => x.Id).FirstOrDefaultAsync();

        record.Should().NotBeNull();
        record!.Guid.Should().Be(id);
    }
}