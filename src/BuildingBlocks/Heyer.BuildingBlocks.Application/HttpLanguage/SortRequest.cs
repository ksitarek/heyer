namespace Heyer.BuildingBlocks.Application.HttpLanguage;

public record SortRequest(string By, SortOrder Order)
{
    public static SortRequest From(string by, string order) =>
        new(by,
            order.ToLowerInvariant() switch
            {
                "asc" => SortOrder.Ascending,
                "desc" => SortOrder.Descending,
                _ => throw new ArgumentException("Invalid sort order")
            });
}