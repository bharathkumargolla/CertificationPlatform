using Certification.Application.Common.Models;

namespace Certification.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagedRequest pagedRequest)
    {
        var skip = (pagedRequest.PageNumber - 1) * pagedRequest.PageSize;

        return query.Skip(skip).Take(pagedRequest.PageSize);
    }

    // TODO: ApplySorting<T>(this IQueryable<T> query, SortRequest sortRequest) - not implemented in this task.
    // TODO: ApplySearch<T>(this IQueryable<T> query, SearchRequest searchRequest) - not implemented in this task.
}
