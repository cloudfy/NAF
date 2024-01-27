namespace Naf.AspNetCore.Mvc.Paging;

public class CollectionPagedResult<T> : PagedResult<T>
{
    internal CollectionPagedResult(
        T[] results) : base(results)
    {
    }
}
