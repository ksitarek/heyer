using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Infrastructure;

internal static class HiringTestData
{
    private const string Company1 = "A62C048C-8E0F-41E2-84D4-BD061F9DDE97";
    private const string Company2 = "0692183B-CE56-432D-88B5-B59280A678C5";

    public static void Seed(HiringDbContext context, string companyId)
    {
        if (companyId == Company1)
        {
            SeedCompany1(context);
        }
        else if (companyId == Company2)
        {
            SeedCompany2(context);
        }
    }

    private static void SeedCompany1(HiringDbContext context)
    {
        var jobOffer = JobOffer.CreateNew(
            new JobOfferId(Guid.Parse("A2916A30-535B-4F2E-A2AA-4FD5CEA7B9D6")),
            "Offer #1",
            "Job description #1",
            RemoteWork.Yes);

        jobOffer.SetOfficeLocation(new OfficeLocation("Warsaw", "Poland"));
        jobOffer.AddContractDetails(
            new ContractDetails(EmploymentType.B2B, new SalaryRange(true, 10000, 20000), 8, 8));
        jobOffer.AddContractDetails(new ContractDetails(EmploymentType.ContractOfEmployment,
                                                        new SalaryRange(true, 8000, 16000),
                                                        8,
                                                        8));
        jobOffer.SetRequirements(ExperienceLevel.Mid,
                                 new Dictionary<string, SkillLevel>
                                 {
                                     { "C#", SkillLevel.Mid }, { "SQL", SkillLevel.Mid }
                                 });

        jobOffer.Publish();

        context.JobOffers.Add(jobOffer);
        context.SaveChanges();
    }

    private static void SeedCompany2(HiringDbContext context)
    {
        var jobOffer = JobOffer.CreateNew(
            new JobOfferId(Guid.Parse("3D6C9BDC-1C7D-4418-B79D-672C48652350")),
            "Offer #2",
            "Job description #2",
            RemoteWork.Yes);

        jobOffer.SetOfficeLocation(new OfficeLocation("Lodz", "Poland"));
        jobOffer.AddContractDetails(
            new ContractDetails(EmploymentType.B2B, new SalaryRange(false, 10000, 20000), 8, 8));
        jobOffer.SetRequirements(ExperienceLevel.Mid,
                                 new Dictionary<string, SkillLevel>
                                 {
                                     { "C#", SkillLevel.Mid }, { "SQL", SkillLevel.Mid }
                                 });

        jobOffer.Publish();

        context.JobOffers.Add(jobOffer);
        context.SaveChanges();
    }
}