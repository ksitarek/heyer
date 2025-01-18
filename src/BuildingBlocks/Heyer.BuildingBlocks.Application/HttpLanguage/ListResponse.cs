namespace Heyer.BuildingBlocks.Application.HttpLanguage;

public record ListResponse<T>(int PageSize, long TotalCount, IEnumerable<T> Items)
{
    public static ListResponse<T> Create(IEnumerable<T> items, int pageSize, long totalCount) =>
        new(pageSize, totalCount, items);
}