using Heyer.Modules.JobBoard.Domain.Candidates;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence;

internal class CandidatesContext : DbContext
{
    public DbSet<Candidate> Candidates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly((typeof(CandidatesContext).Assembly));
    }
}