using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naf.AspNetCore.Mvc.Paging.Internals;

internal sealed class UriQueryBuilder
{
    private readonly UriBuilder _currentUri;
    private readonly Dictionary<string, StringValues> _currentQueryString;

    private UriQueryBuilder(UriBuilder currentUri, Dictionary<string, StringValues> currentQueryString)
    {
        _currentUri = currentUri;
        _currentQueryString = currentQueryString;
    }

    internal UriQueryBuilder AddOrReplace(string key, object value)
    {
        if (_currentQueryString.ContainsKey(key))
            _currentQueryString[key] = value.ToString();
        else
            _currentQueryString.Add(key, value.ToString());

        return this;
    }

    internal Uri Uri
    {
        get
        {
            var queryString = QueryString.Create(_currentQueryString);
            _currentUri.Query = queryString.ToUriComponent();
            return _currentUri.Uri;
        }
    }
    internal static UriQueryBuilder FromRequest(HttpRequest request)
    {
        var currentQueryString = request.Query.ToDictionary();
        var currentUri = new UriBuilder(
            request.Scheme
            , request.Host.Host
            , request.Host.Port ?? (request.Scheme == "https" ? 443 : 80)
            , request.Path);
        
        return new UriQueryBuilder(currentUri, currentQueryString);

    }
}