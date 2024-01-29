using Naf.AspNetCore.Mvc.Paging.Abstracts;

namespace Naf.AspNetCore.Mvc.Paging;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">Type of collection to return.</typeparam>
public sealed class OffsetPagedResult<T> : PagedResult<T>
{
    internal OffsetPagedResult(
        T[] results
        , bool hasMore
        , string? nextOffset = null) : base(results)
    {
        HasMore = hasMore;
        NextOffset = hasMore ? nextOffset : null;
    }

    public string? NextOffset { get; }
    public bool HasMore { get; }
}
