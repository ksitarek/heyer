using Heyer.BuildingBlocks.Application.HttpLanguage;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Infrastructure.Npgsql;
using Heyer.Modules.Hiring.Domain.Candidates;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.Hiring.Infrastructure.Persistence;

internal class HiringDbContext : DbContext, IInboxContext, IOutboxContext
{
    public HiringDbContext(DbContextOptions<HiringDbContext> options) : base(options)
    {
    }

    public DbSet<Candidate> Candidates { get; init; }

    public DbSet<InboxMessage> InboxMessages { get; init; }

    public DbSet<JobOffer> JobOffers { get; init; }
    public DbSet<OutboxMessage> OutboxMessages { get; init; }

    public Task<long> GetTotalCount(FilteredListRequest filteredListRequest,
                                    CancellationToken cancellationToken = default) =>
        JobOffers
            .LongCountAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTimeOffset))
                {
                    property.SetValueConverter(new DateTimeOffsetConverter());
                }
                else if (property.ClrType == typeof(DateTimeOffset?))
                {
                    property.SetValueConverter(new NullableDateTimeOffsetConverter());
                }
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}