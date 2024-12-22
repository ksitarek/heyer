using Heyer.Modules.JobBoard.Domain.Candidates;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence;

internal class JobBoardContext : DbContext
{
    public JobBoardContext(DbContextOptions<JobBoardContext> options) : base(options)
    {
    }

    public DbSet<Candidate> Candidates { get; init; }
    public DbSet<JobOffer> JobOffers { get; init; }


    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
        configurationBuilder.Conventions.Remove<RelationshipDiscoveryConvention>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(JobBoardContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}