using Microsoft.AspNetCore.Mvc;
using Naf.AspNetCore.Mvc.Paging;
using Naf.AspNetCore.Mvc.Paging.Abstracts;
using Naf.AspNetCore.Mvc.Paging.Internals;
namespace Naf.AspNetCore.Mvc;

public static class ControllerExtensions
{
    /// <summary>
    /// Returns the current paging information for the request, or defaults.
    /// </summary>
    /// <param name="controller">Required.</param>
    /// <returns><see cref="PagingInformation"/></returns>
    public static PagingInformation GetPagingDetails(this ControllerBase controller)
    {
        return new PagingInformation(controller);
    }
    /// <summary>
    /// Creates a <see cref="CollectionPagedResult{T}"/> object that includes the collection of results 
    /// and the paging information.
    /// <para>Use <see cref="GetPagingDetails"/> for current take/skip to determine which <paramref name="resultCollection"/> to return..</para>
    /// </summary>
    /// <typeparam name="T">Type of collection.</typeparam>
    /// <param name="controller">Required.</param>
    /// <param name="resultCollection">Required. Current results to return.</param>
    /// <param name="totalCount">Required. Total count of results for all pages.</param>
    /// <exception cref="PageOutOfRangeException"></exception>
    /// <returns><see cref="CollectionPagedResult{T}"/></returns>
    [NonAction]
    public static PagedResult<T> OkCollectionPagingResult<T>(
        this ControllerBase controller
        , T[] resultCollection
        , int totalCount)
    {
        var queryBuilder = UriQueryBuilder.FromRequest(controller.Request);

        return new CollectionPagedResult<T>(
            resultCollection
            , totalCount
            , controller.GetPagingDetails().CurrentPage
            , controller.GetPagingDetails().PageSize
            , queryBuilder);
    }

    [NonAction]
    public static PagedResult<T> OkOffsetPagingResult<T>(
        this ControllerBase controller
        , T[] resultCollection)
    {
        return new OffsetPagedResult<T>(
            resultCollection
            , false);
    }
}
