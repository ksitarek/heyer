namespace Heyer.BuildingBlocks.Application.HttpLanguage;

public record FilteredListRequest(int Page, int PageSize, SortRequest? Sort)
{
    public int PageIx => Page - 1;
}

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

public enum SortOrder
{
    Ascending = 0,
    Descending = 1
}