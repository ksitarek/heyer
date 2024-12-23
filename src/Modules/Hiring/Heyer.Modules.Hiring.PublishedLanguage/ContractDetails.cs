using System.Text.Json.Serialization;

namespace Heyer.Modules.Hiring.PublishedLanguage;

public record ContractDetails
{
    public EmploymentType EmploymentType { get; private set; }
    public SalaryRange SalaryRange { get; private set; } = null!;
    public int TimeNumerator { get; private set; }
    public int TimeDenominator { get; private set; }

    public ContractDetails()
    {
    }

    [JsonConstructor]
    public ContractDetails(EmploymentType employmentType,
                           SalaryRange salaryRange,
                           int timeNumerator,
                           int timeDenominator)
    {
        EmploymentType = employmentType;
        SalaryRange = salaryRange;
        TimeNumerator = timeNumerator;
        TimeDenominator = timeDenominator;
    }
}