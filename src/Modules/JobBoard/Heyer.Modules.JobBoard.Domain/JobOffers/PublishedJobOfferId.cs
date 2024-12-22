namespace Heyer.Modules.JobBoard.Domain.JobOffers;

public record PublishedJobOfferId
{
    public Guid Guid { get; private set; }

    private PublishedJobOfferId()
    {
    }

    public PublishedJobOfferId(Guid guid) => Guid = guid;

    public static PublishedJobOfferId CreateNew() => new(Guid.NewGuid());
}