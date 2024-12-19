using Heyer.Modules.JobBoard.Domain.Candidates;
using Heyer.Modules.JobBoard.Domain.Companies;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence.EntityConfigurations;

public class JobOfferEntityTypeConfiguration : IEntityTypeConfiguration<JobOffer>
{
    public void Configure(EntityTypeBuilder<JobOffer> builder)
    {
        builder.ToCollection("JobOffers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Guid, x => new JobOfferId(x));

        builder.Property("_offerSummary")
            .HasElementName("OfferSummary")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property("_jobDescription")
            .HasElementName("JobDescription")
            .IsRequired();

        builder.OwnsOne<CompanyDetails>("_companyDetails", cd =>
        {
            cd.HasElementName("CompanyDetails");
            cd.Property(x => x.CompanyId)
                .HasConversion(x => x.Id, x => new CompanyId(x));
        });

        builder.OwnsOne<OfficeLocation>("_location", l => { l.HasElementName("Location"); });

        builder.OwnsOne<Requirements>("_requirements", r =>
        {
            r.HasElementName("Requirements");
            r.Property("ExperienceLevel")
                .IsRequired();

            r.OwnsMany<Skill>("Skills", s =>
            {
                s.Property(x => x.Label)
                    .IsRequired();

                s.Property(x => x.Level)
                    .IsRequired();
            });
        });

        builder.OwnsMany<CandidateId>("_candidates", c => { c.HasElementName("Candidates"); });
    }
}