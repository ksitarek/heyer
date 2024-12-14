using Heyer.Modules.Candidates.Domain.Candidates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Heyer.Modules.Candidates.IntegrationEvents.Persistence.Candidates;

public class CandidateEntityTypeConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToCollection("Candidates");
        
        builder.HasKey(x => x.Id);

        builder.Property("_firstName").HasElementName("FirstName").IsRequired();
        builder.Property("_lastName").HasElementName("LastName").IsRequired();
        builder.Property("_email").HasElementName("Email").IsRequired();
        builder.Property("_resumeKey").HasElementName("ResumeKey").IsRequired();
        
        builder.HasIndex("Email").IsUnique();
    }
}