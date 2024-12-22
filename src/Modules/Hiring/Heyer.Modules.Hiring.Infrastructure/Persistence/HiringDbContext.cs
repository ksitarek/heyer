using Heyer.Modules.Hiring.Domain.Candidates;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;

namespace Heyer.Modules.Hiring.Infrastructure.Persistence;

public class HiringDbContext : DbContext
{
    public DbSet<Candidate> Candidates { get; init; }
    public DbSet<JobOffer> JobOffers { get; init; }
}