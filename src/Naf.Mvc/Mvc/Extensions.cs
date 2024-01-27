using Microsoft.AspNetCore.Mvc;
using Naf.AspNetCore.Mvc.Paging;
using System.Linq;

namespace Naf.AspNetCore.Mvc;

public static class Extensions
{    
    public static ActionResult OkCollectionPagingResult<T>(
        this ControllerBase controller
        , T[] resultCollection)
    {
        return new CollectionPagedResult<T>(
            resultCollection);
    }

    public static ActionResult OkOffsetPagingResult<T>(
        this ControllerBase controller
        , T[] resultCollection)
    {        
        return new OffsetPagedResult<T>(
            resultCollection
            , false);
    }
}
