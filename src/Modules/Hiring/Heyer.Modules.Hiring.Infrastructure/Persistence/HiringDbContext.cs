using Heyer.Modules.Hiring.Domain.Candidates;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Heyer.Modules.Hiring.Infrastructure.Persistence;

public class HiringDbContext : DbContext
{
    public HiringDbContext(DbContextOptions<HiringDbContext> options) : base(options)
    {
    }

    public DbSet<Candidate> Candidates { get; init; }
    public DbSet<JobOffer> JobOffers { get; init; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) =>
        configurationBuilder.Conventions.Remove<RelationshipDiscoveryConvention>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}