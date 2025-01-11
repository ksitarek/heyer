using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence;

internal class JobBoardContext : DbContext, IInboxContext, IOutboxContext
{
    public JobBoardContext(DbContextOptions<JobBoardContext> options) : base(options)
    {
    }

    public DbSet<InboxMessage> InboxMessages { get; init; }
    public DbSet<OutboxMessage> OutboxMessages { get; init; }

    public DbSet<PublishedJobOffer> PublishedJobOffers { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("job_board");

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}