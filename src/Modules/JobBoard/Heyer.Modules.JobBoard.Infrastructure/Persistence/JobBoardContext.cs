using Heyer.Modules.JobBoard.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence;

internal class JobBoardContext : DbContext
{
    public DbSet<JobOffer> JobOffers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly((typeof(JobBoardContext).Assembly));
    }
}