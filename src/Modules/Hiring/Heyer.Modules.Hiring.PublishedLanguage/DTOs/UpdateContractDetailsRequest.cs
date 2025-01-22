namespace Heyer.Modules.Hiring.PublishedLanguage.DTOs;

public record UpdateContractDetailsRequest(
    Guid JobOfferId,
    EmploymentType EmploymentType,
    SalaryRange SalaryRange,
    int TimeNumerator,
    int TimeDenominator);