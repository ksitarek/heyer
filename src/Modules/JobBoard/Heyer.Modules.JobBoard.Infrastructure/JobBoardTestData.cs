using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Modules.JobBoard.Infrastructure.Persistence;

namespace Heyer.Modules.JobBoard.Infrastructure;

internal static class JobBoardTestData
{
    public static void Seed(JobBoardContext jobBoardContext)
    {
        var publishedJobOffer1 = PublishedJobOffer.CreateNew(
            new PublishedJobOfferId(Guid.Parse("A2916A30-535B-4F2E-A2AA-4FD5CEA7B9D6")),
            new CompanyDetails(Guid.Parse("A62C048C-8E0F-41E2-84D4-BD061F9DDE97"),
                               "ACME Corporation A62C048C-8E0F-41E2-84D4-BD061F9DDE97"),
            "Offer #1",
            "Job description #1",
            RemoteWork.Yes,
            new List<ContractDetails>
            {
                new(EmploymentType.B2B, new SalaryRange(true, 10000, 20000), 8, 8),
                new(EmploymentType.ContractOfEmployment, new SalaryRange(true, 8000, 16000), 8, 8)
            },
            new OfficeLocation("Warsaw", "Poland"),
            new DateTimeOffset(2024, 12, 31, 11, 11, 11, TimeSpan.Zero),
            null,
            new Requirements(ExperienceLevel.Mid,
                             new List<Skill> { new("C#", SkillLevel.Mid), new("SQL", SkillLevel.Mid) }));

        var publishedJobOffer2 = PublishedJobOffer.CreateNew(
            new PublishedJobOfferId(Guid.Parse("3D6C9BDC-1C7D-4418-B79D-672C48652350")),
            new CompanyDetails(Guid.Parse("0692183B-CE56-432D-88B5-B59280A678C5"),
                               "ACME Corporation 0692183B-CE56-432D-88B5-B59280A678C5"),
            "Offer #2",
            "Job description #2",
            RemoteWork.Yes,
            new List<ContractDetails> { new(EmploymentType.B2B, new SalaryRange(true, 10000, 20000), 8, 8) },
            new OfficeLocation("Lodz", "Poland"),
            new DateTimeOffset(2024, 12, 30, 22, 22, 22, TimeSpan.Zero),
            null,
            new Requirements(ExperienceLevel.Mid,
                             new List<Skill> { new("C#", SkillLevel.Mid), new("SQL", SkillLevel.Mid) }));

        jobBoardContext.PublishedJobOffers.AddRange(publishedJobOffer1, publishedJobOffer2);

        jobBoardContext.SaveChanges();
    }
}