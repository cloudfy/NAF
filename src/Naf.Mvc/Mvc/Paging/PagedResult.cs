using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Naf.AspNetCore.Mvc.Paging;

public abstract class PagedResult<T> 
    : ActionResult, IStatusCodeActionResult
{
    internal PagedResult(T[] results) : base()
    {
        Results = results;
        StatusCode = (int)StatusCodes.Status200OK;
    }
    public T[] Results { get; }

    public int? StatusCode { get; }
}
