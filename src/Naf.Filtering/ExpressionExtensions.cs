using Naf.Filtering.Query;

namespace Naf.Filtering;

public static class ExpressionExtensions
{
    /// <summary>
    /// Creates an expression from the provided <see cref="filterQueryOption"/>
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="queryFilter"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Expression<Func<TSource, bool>> CreateExpression<TSource>(
        this QueryNode queryFilter)
    {
        if (queryFilter is null) throw new ArgumentNullException(nameof(queryFilter));

        var parameter = Expression.Parameter(typeof(TSource), typeof(TSource).Name);

        var filterClause = InternalExtensions.CreateExpression(queryFilter, parameter);

        return Expression.Lambda<Func<TSource, bool>>(filterClause, parameter);
    }
}
