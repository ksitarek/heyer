namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record JobOfferId(Guid Guid)
{
    public static JobOfferId CreateNew()
    {
        return new JobOfferId(Guid.NewGuid());
    }
}