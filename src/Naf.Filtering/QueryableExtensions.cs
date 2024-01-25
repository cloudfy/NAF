using Naf.Filtering.Query;

namespace Naf.Filtering;

public static partial class QueryableExtensions
{
    /// <summary>
    /// Filters a sequence of values based on the provided <see cref="queryFilter"/>
    /// </summary>
    /// <param name="source">The queryable source</param>
    /// <param name="queryFilter">The filter options to apply</param>
    /// <returns>The filtered data source</returns>
    /// <exception cref="ArgumentNullException">Source or filter are null</exception>
    public static IQueryable<TSource> ApplyFilterExpression<TSource>(
        this IQueryable<TSource> source
        , QueryNode? queryFilter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        if (queryFilter is null) return source;

        var parameter = Expression.Parameter(source.ElementType, source.ElementType.Name);

        var filterClause = InternalExtensions.CreateExpression(queryFilter, parameter);

        var lambda = Expression.Lambda<Func<TSource, bool>>(filterClause, parameter);

        return source.Where(lambda);
    }
}