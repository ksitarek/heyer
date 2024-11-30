namespace Heyer.BuildingBlocks.Infrastructure;

public class SystemDateTime : IDateTimeProvider
{
    public DateTimeOffset UtcNow() => DateTimeOffset.UtcNow;
}