using Microsoft.AspNetCore.Mvc;

namespace Naf.AspNetCore.Mvc.Paging;

public sealed class PagingInformation
{
    private readonly ControllerBase _controller;

    internal PagingInformation(
        ControllerBase controller) => _controller = controller;

    /// <summary>
    /// Gets the current page from the page parameter. Defaults to 1.
    /// </summary>
    public int CurrentPage
    {
        get
        {
            if (_controller.Request.Query.TryGetValue<int>("page", out var page))
                return page > 0 ? page : 1;
            return 1;
        }
    }
    /// <summary>
    /// Gets the default page size. Defaults to 50.
    /// </summary>
    public int PageSize 
    {
        get 
        {
            if (_controller.Request.Query.TryGetValue<int>("pagesize", out var pageSize))
                return pageSize > 0 && pageSize <= 100 ? pageSize : 50;
            return 50;
        }
    }
    public int Skip => (CurrentPage - 1) * PageSize;
    public bool PageIsSpecified => _controller.Request.Query.ContainsKey("page");
    public bool PageSizeIsSpecified => _controller.Request.Query.ContainsKey("pagesize");

}
