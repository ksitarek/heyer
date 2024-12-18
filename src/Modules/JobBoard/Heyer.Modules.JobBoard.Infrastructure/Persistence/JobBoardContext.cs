using Heyer.Modules.JobBoard.Domain.Candidates;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence;

internal class JobBoardContext : DbContext
{
    public DbSet<JobOffer> JobOffers { get; set; }
    public DbSet<Candidate> Candidates { get; set; }

    public JobBoardContext(DbContextOptions<JobBoardContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly((typeof(JobBoardContext).Assembly));
    }
}