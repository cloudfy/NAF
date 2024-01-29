using System;

namespace Naf.AspNetCore.Mvc.Paging;

public sealed class CollectionPageInfo
{
    internal CollectionPageInfo(int currentPage, int pageSize, int totalResults, int totalPages)
    {
        Page = currentPage;
        PageSize = pageSize;
        TotalResults = totalResults;
        TotalPages = totalPages;
    }
    public int PageSize { get; }
    public int TotalResults { get; }
    public int TotalPages { get; }
    public int Page { get; }

    public string? Previous { get; private set; }
    public string? Next { get; private set; }

    internal void SetUris(string? nextPage, string? previousPage)
    {
        Previous = previousPage;
        Next = nextPage;
    }
}