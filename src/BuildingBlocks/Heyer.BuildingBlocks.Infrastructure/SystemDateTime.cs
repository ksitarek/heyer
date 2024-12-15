namespace Heyer.BuildingBlocks.Infrastructure;

public class SystemDateTime : IDateTimeProvider
{
    public DateTime UtcNow() => DateTime.UtcNow;
}