using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Threading.Tasks;

namespace Naf.AspNetCore.Mvc.Paging.Abstracts;

[DefaultStatusCode(_defaultStatusCode)]
public abstract class PagedResult<T>
    : IActionResult
{
    private const int _defaultStatusCode = StatusCodes.Status200OK;

    internal PagedResult(T[] results)
    {
        Results = results;
    }
    public T[] Results { get; }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(this)
        {
            StatusCode = StatusCodes.Status200OK
        };

        await objectResult.ExecuteResultAsync(context);
    }
}