namespace Heyer.API.Client.PublishedLanguage;

public record JobOfferDetails(
    Guid Id,
    JobOfferDetails.CompanyDetailsDto CompanyDetails,
    string OfferSummary,
    string JobDescription,
    JobOfferDetails.LocationDto LocationD,
    RemoteWork RemoteWork,
    JobOfferDetails.RequirementsDto Requirements,
    List<JobOfferDetails.ContractDetailsDto> ContractDetails)
{
    public record ContractDetailsDto(
        EmploymentType EmploymentType,
        SalaryRangeDto? SalaryRange);

    public record CompanyDetailsDto(Guid CompanyId, string CompanyName);

    public record LocationDto(string City, string Country);

    public record SalaryRangeDto(decimal From, decimal To);

    public record RequirementsDto(ExperienceLevel ExperienceLevel, List<SkillDto> Skills);

    public record SkillDto(string Label, SkillLevel Level);

    public enum EmploymentType
    {
        ContractOfEmployment,
        B2B
    }

    public enum ExperienceLevel
    {
        Junior,
        Mid,
        Senior,
        CLevel
    }

    public enum SkillLevel
    {
        NiceToHave,
        Junior,
        Mid,
        Senior,
        Expert
    }
}