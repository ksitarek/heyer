using Heyer.Modules.Hiring.Domain.Candidates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Heyer.Modules.Hiring.Infrastructure.Persistence.EntityConfiguration;

public class CandidateEntityTypeConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToCollection("Candidates");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Guid, x => new CandidateId(x));

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.IncludeInCandidatePool)
            .IsRequired();

        // builder.Property(x => x.Attributes)
        //     .HasElementName("Attributes");

        // builder.OwnsOne<Email>(x => x.Email, e => { e.HasElementName("Email"); });
        //
        // builder.OwnsOne<ResumeKey>(x => x.ResumeKey, rk => { rk.HasElementName("ResumeKey"); });
    }
}