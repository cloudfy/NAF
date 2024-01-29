using Naf.AspNetCore.Mvc.Paging.Abstracts;
using System;

namespace Naf.AspNetCore.Mvc.Paging;

/// <summary>
/// When executed, returns a collection of results including <see cref="CollectionPageInfo"/> by producing 
/// a <see cref="Microsoft.AspNetCore.Http.StatusCodes.Status200OK"/> response.
/// </summary>
/// <typeparam name="T">Type of collection to return.</typeparam>
public sealed class CollectionPagedResult<T> : PagedResult<T>
{
    internal CollectionPagedResult(
        T[] results
        , int totalCount
        , int currentPage
        , int pageSize
        , Internals.UriQueryBuilder queryBuilder) : base(results)
    {
        if (results.Length > pageSize)
            throw new PageOutOfRangeException(
                $"Results larger than page size ({pageSize}). Limits the results to include in the response to the maximum of the page size.");

        var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
        Paging = new CollectionPageInfo(currentPage, pageSize, totalCount, totalPages);
        if (Paging.Page > Paging.TotalPages)
            throw new PageOutOfRangeException(
                $"Page {currentPage} is larger than the total number pages ({totalPages}).");

        string? nextPage = null;
        string? previousPage = null;
        if (Paging.Page >= 1 && Paging.Page < Paging.TotalPages)
        {
            queryBuilder.AddOrReplace("page", currentPage + 1);
            nextPage = queryBuilder.Uri.ToString();
        }
        if (Paging.Page >= 2 && Paging.Page <= Paging.TotalPages)
        {
            queryBuilder.AddOrReplace("page", currentPage - 1);
            previousPage = queryBuilder.Uri.ToString();
        }
        Paging.SetUris(nextPage, previousPage);
    }

    public CollectionPageInfo Paging { get; }
}
