using System.Linq.Dynamic.Core;
using Heyer.BuildingBlocks.Application.HttpLanguage;

namespace Heyer.BuildingBlocks.Infrastructure.Extensions;

public static class OrderExtensions
{
    public static IQueryable<T> Sort<T>(this IQueryable<T> query, SortRequest? order)
    {
        if (order is null)
        {
            return query;
        }

        var sortDirection = order.Order switch
        {
            SortOrder.Ascending => "asc",
            SortOrder.Descending => "desc",
            _ => "asc"
        };

        var sortParam = $"{order.By} {sortDirection}";

        return query.OrderBy(sortParam);
    }
}