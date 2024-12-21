using Heyer.API.Client.PublishedLanguage;
using Heyer.Modules.JobBoard.Application.JobOffers.Create;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using RemoteWork = Heyer.Modules.JobBoard.Domain.JobOffers.RemoteWork;

namespace Heyer.Modules.JobBoard.Application.Mapping;

public static class JobOfferMappings
{
    public static JobOfferDetails.ContractDetailsDto MapContractDetails(this ContractDetails contractDetails)
        => new(contractDetails.EmploymentType.MapEmploymentType(),
               contractDetails.SalaryRange?.MapSalaryRange());

    public static CreateJobOffer MapToCommand(this CreateJobOfferRequest request) =>
        new(
            request.OfferSummary,
            request.JobDescription,
            request.RemoteWork.MapRemoteWork());

    public static JobOfferDetails MapToJobOfferDetails(this JobOffer jobOffer) =>
        new(jobOffer.Id.Guid,
            jobOffer.CompanyDetails.MapCompanyDetails(),
            jobOffer.OfferSummary,
            jobOffer.JobDescription,
            jobOffer.Location!.MapLocation(),
            jobOffer.RemoteWork.MapRemoteWork(),
            jobOffer.Requirements!.MapRequirements(),
            jobOffer.ContractsDetails!.Select(contractDetails => contractDetails.MapContractDetails()).ToList());

    private static JobOfferDetails.CompanyDetailsDto MapCompanyDetails(this CompanyDetails companyDetails)
        => new(companyDetails.CompanyId.Id, companyDetails.Name);

    private static JobOfferDetails.EmploymentType MapEmploymentType(this EmploymentType employmentType)
        => employmentType switch
        {
            EmploymentType.ContractOfEmployment => JobOfferDetails.EmploymentType.ContractOfEmployment,
            EmploymentType.B2B => JobOfferDetails.EmploymentType.B2B,
            _ => throw new ArgumentOutOfRangeException(nameof(employmentType), employmentType, null)
        };

    private static JobOfferDetails.ExperienceLevel MapExperienceLevel(
        this ExperienceLevel experienceLevel)
        => experienceLevel switch
        {
            ExperienceLevel.Junior => JobOfferDetails.ExperienceLevel.Junior,
            ExperienceLevel.Mid => JobOfferDetails.ExperienceLevel.Mid,
            ExperienceLevel.Senior => JobOfferDetails.ExperienceLevel.Senior,
            ExperienceLevel.CLevel => JobOfferDetails.ExperienceLevel.CLevel,
            _ => throw new ArgumentOutOfRangeException(nameof(experienceLevel), experienceLevel, null)
        };

    private static JobOfferDetails.LocationDto MapLocation(this OfficeLocation location)
        => new(location.City, location.Country);

    private static API.Client.PublishedLanguage.RemoteWork MapRemoteWork(this RemoteWork remoteWork) =>
        remoteWork switch
        {
            RemoteWork.Unknown => API.Client.PublishedLanguage.RemoteWork.Unknown,
            RemoteWork.No => API.Client.PublishedLanguage.RemoteWork.No,
            RemoteWork.Hybrid => API.Client.PublishedLanguage.RemoteWork.Hybrid,
            RemoteWork.Yes => API.Client.PublishedLanguage.RemoteWork.Yes,
            _ => throw new ArgumentOutOfRangeException(nameof(remoteWork), remoteWork, null)
        };

    private static RemoteWork MapRemoteWork(this API.Client.PublishedLanguage.RemoteWork remoteWork) =>
        remoteWork switch
        {
            API.Client.PublishedLanguage.RemoteWork.Unknown => RemoteWork.Unknown,
            API.Client.PublishedLanguage.RemoteWork.No => RemoteWork.No,
            API.Client.PublishedLanguage.RemoteWork.Hybrid => RemoteWork.Hybrid,
            API.Client.PublishedLanguage.RemoteWork.Yes => RemoteWork.Yes,
            _ => throw new ArgumentOutOfRangeException(nameof(remoteWork), remoteWork, null)
        };

    private static JobOfferDetails.RequirementsDto MapRequirements(this Requirements requirements)
        => new(requirements.ExperienceLevel.MapExperienceLevel(),
               requirements.Skills?.Select(skill => skill.MapSkill()).ToList() ?? new List<JobOfferDetails.SkillDto>());

    private static JobOfferDetails.SalaryRangeDto MapSalaryRange(this SalaryRange salaryRange)
        => new(salaryRange.From, salaryRange.To);

    private static JobOfferDetails.SkillDto MapSkill(this Skill skill)
        => new(skill.Label, skill.Level.MapSkillLevel());

    private static JobOfferDetails.SkillLevel MapSkillLevel(this SkillLevel skillLevel)
        => skillLevel switch
        {
            SkillLevel.NiceToHave => JobOfferDetails.SkillLevel.NiceToHave,
            SkillLevel.Junior => JobOfferDetails.SkillLevel.Junior,
            SkillLevel.Mid => JobOfferDetails.SkillLevel.Mid,
            SkillLevel.Senior => JobOfferDetails.SkillLevel.Senior,
            SkillLevel.Expert => JobOfferDetails.SkillLevel.Expert,
            _ => throw new ArgumentOutOfRangeException(nameof(skillLevel), skillLevel, null)
        };
}