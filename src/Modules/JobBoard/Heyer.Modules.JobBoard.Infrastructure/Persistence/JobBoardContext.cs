using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Infrastructure.Npgsql;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence;

internal class JobBoardContext : DbContext, IInboxContext, IOutboxContext
{
    public JobBoardContext(DbContextOptions<JobBoardContext> options) : base(options)
    {
    }

    public DbSet<InboxMessage> InboxMessages { get; init; } = null!;
    public DbSet<OutboxMessage> OutboxMessages { get; init; } = null!;
    public DbSet<PublishedJobOffer> PublishedJobOffers { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("job_board");

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