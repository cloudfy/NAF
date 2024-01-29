namespace Naf.AspNetCore.Mvc;

internal static class InternalExtensions
{
    internal static bool TryGetValue<T>(
        this Microsoft.AspNetCore.Http.IQueryCollection query, string key, out T value)
    {
        // todo: check for null 
        if (query.TryGetValue(key, out var objectValue))
        {
            value = (T)System.Convert.ChangeType(objectValue.ToString(), typeof(T));
            return true;
        }
        value = default;
        return false;
    }
}
