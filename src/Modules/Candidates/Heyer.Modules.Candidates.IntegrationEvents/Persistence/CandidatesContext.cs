using Microsoft.EntityFrameworkCore;
using Heyer.Modules.Candidates.Domain.Candidates;

namespace Heyer.Modules.Candidates.IntegrationEvents.Persistence;

internal class CandidatesContext : DbContext
{
    public DbSet<Candidate> Candidates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly((typeof(CandidatesContext).Assembly));
    }
}