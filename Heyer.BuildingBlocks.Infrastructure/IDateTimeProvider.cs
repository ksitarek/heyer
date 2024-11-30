namespace Heyer.BuildingBlocks.Infrastructure;

public interface IDateTimeProvider
{
    public DateTimeOffset UtcNow();
}