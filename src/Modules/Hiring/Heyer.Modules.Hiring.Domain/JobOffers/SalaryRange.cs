namespace Heyer.Modules.Hiring.Domain.JobOffers;

public record SalaryRange
{
    public bool IsPublished { get; private set; }
    public decimal From { get; private set; }
    public decimal To { get; private set; }

    public SalaryRange()
    {
    }

    public SalaryRange(bool isPublished, decimal from, decimal to)
    {
        IsPublished = isPublished;
        From = from;
        To = to;
    }
}