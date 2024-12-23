using Heyer.Modules.Hiring.PublishedLanguage;
using Heyer.Modules.JobBoard.Domain.Companies;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Heyer.Modules.JobBoard.Infrastructure.Persistence.EntityConfigurations;

public class PublishedJobOfferEntityTypeConfiguration : IEntityTypeConfiguration<PublishedJobOffer>
{
    public void Configure(EntityTypeBuilder<PublishedJobOffer> builder)
    {
        builder.ToCollection("JobOffers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Guid, x => new PublishedJobOfferId(x));

        builder.Property(x => x.OfferSummary)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.JobDescription)
            .IsRequired();

        builder.OwnsOne(x => x.CompanyDetails);

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