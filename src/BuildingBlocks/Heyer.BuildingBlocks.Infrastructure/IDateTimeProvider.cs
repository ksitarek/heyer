namespace Heyer.BuildingBlocks.Infrastructure;

public interface IDateTimeProvider
{
    public DateTime UtcNow();
}