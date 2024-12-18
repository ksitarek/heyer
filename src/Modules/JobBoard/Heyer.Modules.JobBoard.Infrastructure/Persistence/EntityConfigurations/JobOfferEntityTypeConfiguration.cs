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
            cd.Property(x => x.CompanyId)
                .HasConversion(x => x.Id, x => new CompanyId(x));
        });
        builder.OwnsOne<OfficeLocation>("_location");
        builder.OwnsOne<Requirements>("_requirements", r =>
        {
            r.Property("_experienceLevel")
                .HasElementName("ExperienceLevel")
                .IsRequired();
            
            r.OwnsMany<Skill>("_skills", s =>
            {
                s.Property(x => x.Label)
                    .IsRequired();

                s.Property(x => x.Level)
                    .IsRequired();
            });
        });
        
        builder.OwnsMany<CandidateId>("_candidates");
    }
}

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

        // builder.Property("_email")
        //     .HasElementName("Email")
        //     .IsRequired()
        //     .HasMaxLength(100);
        
        // builder.Property("_resumeKey")
        //     .HasElementName("ResumeKey")
        //     .IsRequired();

        builder.Property("_includeInCandidatePool")
            .HasElementName("IncludeInCandidatePool")
            .IsRequired();

        builder.OwnsOne<Email>("_email");
        builder.OwnsOne<ResumeKey>("_resumeKey");
    }
}