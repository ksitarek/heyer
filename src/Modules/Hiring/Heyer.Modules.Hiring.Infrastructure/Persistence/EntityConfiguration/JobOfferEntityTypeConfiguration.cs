using Heyer.Modules.Hiring.Domain.Companies;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Heyer.Modules.Hiring.Infrastructure.Persistence.EntityConfiguration;

public class JobOfferEntityTypeConfiguration : IEntityTypeConfiguration<JobOffer>
{
    public void Configure(EntityTypeBuilder<JobOffer> builder)
    {
        builder.ToCollection("JobOffers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Guid, x => new JobOfferId(x));

        builder.Property(x => x.OfferSummary)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.JobDescription)
            .IsRequired();

        builder.OwnsOne(x => x.CompanyDetails,
                        cd =>
                        {
                            cd.Property(x => x.CompanyId)
                                .HasConversion(x => x.Id, x => new CompanyId(x));
                        });

        builder.OwnsOne(x => x.Location);

        builder.OwnsOne(x => x.Requirements,
                        r =>
                        {
                            r.Property(x => x.ExperienceLevel)
                                .IsRequired();

                            r.OwnsMany(x => x.Skills,
                                       s =>
                                       {
                                           s.HasElementName("Skills");

                                           s.Property(x => x.Label)
                                               .IsRequired();

                                           s.Property(x => x.Level)
                                               .IsRequired();
                                       });
                        });

        builder.OwnsMany(x => x.ContractsDetails,
                         nb => { nb.OwnsOne<SalaryRange>(x => x.SalaryRange); });
    }
}