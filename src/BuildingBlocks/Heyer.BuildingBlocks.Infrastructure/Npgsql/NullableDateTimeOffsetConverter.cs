using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Heyer.BuildingBlocks.Infrastructure.Npgsql;

public class NullableDateTimeOffsetConverter : ValueConverter<DateTimeOffset?, DateTimeOffset?>
{
    public NullableDateTimeOffsetConverter()
        : base(
            d => d == null ? null : d.Value.ToUniversalTime(),
            d => d == null ? null : d.Value.ToUniversalTime())
    {
    }
}