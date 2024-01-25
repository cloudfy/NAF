using Naf.Net.Internals;
using System;

namespace Naf.Net;

/// <summary>
/// Provides URL path manipulations.
/// </summary>
public sealed class UriPath
{
    /// <summary>
    /// Returns a URL combined by base and path elements. Each element will be seperated by a '/'.
    /// </summary>
    /// <param name="baseUrl">Base http or https url.</param>
    /// <param name="paths">Array of path elements.</param>
    /// <returns></returns>
    public static string Combine(string baseUrl, params string[] paths)
    {
        Helpers.Assert(baseUrl);

        if (!baseUrl.ToLower().StartsWith("http"))
            throw new ArgumentException("Base Url must begin with http or https");

        // remove trailing / on base path
        if (baseUrl.EndsWith("/"))
            baseUrl = baseUrl[..^1];
        if (paths == null)
            return baseUrl;

        foreach (var path in paths)
        {
            if (!path.StartsWith("/"))
                baseUrl += "/" + path;
            else if (path.EndsWith("/") && path.Length > 1)
                baseUrl += path.Substring(0, path.Length - 1);
            else
                baseUrl += path;
        }

        return baseUrl;
    }
}