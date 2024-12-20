namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record ContractDetails
{
    public SalaryRange SalaryRange { get; private set; } = null!;
    public int TimeNumerator { get; private set; }
    public int TimeDenominator { get; private set; }

    public ContractDetails()
    {
    }

    public ContractDetails(SalaryRange salaryRange, int timeNumerator, int timeDenominator)
    {
        SalaryRange = salaryRange;
        TimeNumerator = timeNumerator;
        TimeDenominator = timeDenominator;
    }
}