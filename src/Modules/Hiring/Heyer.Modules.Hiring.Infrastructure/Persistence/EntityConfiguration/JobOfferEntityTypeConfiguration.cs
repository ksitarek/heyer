using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Heyer.Modules.Hiring.Infrastructure.Persistence.EntityConfiguration;

public class JobOfferEntityTypeConfiguration : IEntityTypeConfiguration<JobOffer>
{
    public void Configure(EntityTypeBuilder<JobOffer> builder)
    {
        builder.ToTable("JobOffers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Guid, x => new JobOfferId(x));

        builder.Property(x => x.OfferSummary)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.JobDescription)
            .IsRequired();

        builder.OwnsOne(x => x.Location,
                        l =>
                        {
                            l.Property(x => x.City)
                                .IsRequired()
                                .HasMaxLength(100);

                            l.Property(x => x.Country)
                                .IsRequired()
                                .HasMaxLength(100);
                        });

        builder.OwnsOne(x => x.Requirements,
                        r =>
                        {
                            r.ToTable("JobOfferRequirements");
                            r.Property(x => x.ExperienceLevel)
                                .IsRequired();

                            r.OwnsMany(x => x.Skills,
                                       s =>
                                       {
                                           s.ToTable("Skills");

                                           s.Property(x => x.Label)
                                               .IsRequired();

                                           s.Property(x => x.Level)
                                               .IsRequired();
                                       });
                        });

        builder.OwnsMany(x => x.ContractsDetails,
                         nb =>
                         {
                             nb.ToTable("JobOfferContractsDetails");
                             nb.OwnsOne<SalaryRange>(x => x.SalaryRange);
                         });

        builder.OwnsMany(x => x.Candidates, x => x.ToTable("JobOfferCandidates"));

        /*builder.HasData(new
        {
            ContractDetails =
                new List<ContractDetails>
                {
                    new(EmploymentType.B2B, new SalaryRange(true, 10000, 20000), 8, 8),
                    new(EmploymentType.ContractOfEmployment, new SalaryRange(true, 8000, 16000), 8, 8)
                },
            Id = new JobOfferId(Guid.Parse("269F455B-9A07-4393-A95C-38B9E11E6E5A")),
            OfferSummary = "OfferSummary #1",
            JobDescription = "JobDescription #1",
            Location = new OfficeLocation("Warsaw", "Poland"),
            PublishedAt = new DateTimeOffset(2025, 01, 01, 0, 0, 0, TimeSpan.Zero),
            RemoteWork = RemoteWork.Yes,
            Requirements = new Requirements(ExperienceLevel.Junior,
                                            new List<Skill> { new("C#", SkillLevel.Mid), new("SQL", SkillLevel.Mid) })
        });*/
    }
}