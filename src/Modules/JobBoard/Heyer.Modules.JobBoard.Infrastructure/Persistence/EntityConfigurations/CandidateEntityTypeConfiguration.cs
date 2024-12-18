using Heyer.Modules.JobBoard.Domain.Candidates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence.EntityConfigurations;

public class CandidateEntityTypeConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToCollection("Candidates");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Guid, x => new CandidateId(x));
        
        builder.Property("_firstName")
            .HasElementName("FirstName")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property("_lastName")
            .HasElementName("LastName")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property("_includeInCandidatePool")
            .HasElementName("IncludeInCandidatePool")
            .IsRequired();

        builder.OwnsOne<Email>("_email", e =>
        {
            e.HasElementName("Email");
        });
        
        builder.OwnsOne<ResumeKey>("_resumeKey", rk =>
        {
            rk.HasElementName("ResumeKey");
        });
    }
}