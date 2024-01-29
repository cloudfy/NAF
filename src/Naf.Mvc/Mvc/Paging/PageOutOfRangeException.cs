using System;

namespace Naf.AspNetCore.Mvc.Paging;

/// <summary>
/// Represents errors that occur when the page is out of range of total pages, 
/// or the results is larger than the page size.
/// </summary>
public sealed class PageOutOfRangeException
    : Exception
{
    internal PageOutOfRangeException(string message) : base(message) { }
}
