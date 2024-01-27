namespace Naf.AspNetCore.Mvc.Paging;


public class OffsetPagedResult<T> : PagedResult<T>
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
