using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}