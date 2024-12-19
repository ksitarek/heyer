namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record JobOfferId
{
    public Guid Guid { get; private set; }

    private JobOfferId()
    {
    }

    public JobOfferId(Guid guid) => Guid = guid;

    public static JobOfferId CreateNew() => new(Guid.NewGuid());
}