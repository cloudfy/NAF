using System;

namespace Naf.Core;

public static class DateTimeExtensions
{
    /// <summary>
    /// Returns the last day of the month for the given <paramref name="dateTime"/>.
    /// </summary>
    /// <param name="dateTime">Required.</param>
    /// <returns><see cref="DateTime"/></returns>
    public static DateTime EndOfMonth(this DateTime dateTime)
        => new (dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month)
            , dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, dateTime.Kind);
    /// <summary>
    /// Returns the first day of the month for the given <paramref name="dateTime"/>.
    /// </summary>
    /// <param name="dateTime">Required.</param>
    /// <returns><see cref="DateTime"/></returns>
    public static DateTime BeginningOfMonth(this DateTime dateTime)
        => new (dateTime.Year, dateTime.Month, 1, dateTime.Hour
            , dateTime.Minute, dateTime.Second, dateTime.Millisecond, dateTime.Kind);
    /// <summary>
    /// Returns true if the given <paramref name="other"/> is the same month (and year).
    /// </summary>
    /// <param name="dateTime">Required.</param>
    /// <param name="other">Required. Date to compare.</param>
    /// <param name="emitYear">Optional. If true, does not compare year. Default false.</param>
    /// <returns><see cref="bool"/></returns>
    public static bool IsSameMonth(this DateTime dateTime, DateTime other, bool emitYear = false)
    {
        if (emitYear)
        {
            return dateTime.Month == other.Month;
        }
        else
        {
            return dateTime.Year == other.Year && dateTime.Month == other.Month;
        }
    }

#if NET7_0_OR_GREATER
    /// <summary>
    /// Returns true if the given <paramref name="other"/> is the same month (and year).
    /// </summary>
    /// <param name="dateTime">Required.</param>
    /// <param name="other">Required. Date to compare.</param>
    /// <param name="emitYear">Optional. If true, does not compare year. Default false.</param>
    /// <returns><see cref="bool"/></returns>
    public static bool IsSameMonth(this DateTime dateTime, DateOnly other, bool emitYear = false)
    {
        if (emitYear)
        {
            return dateTime.Month == other.Month;
        }
        else
        {
            return dateTime.Year == other.Year && dateTime.Month == other.Month;
        }
    }
    /// <summary>
    /// Returns a <see cref="DateOnly"/> from the given <paramref name="dateTime"/>.
    /// </summary>
    /// <param name="dateTime">Required.</param>
    /// <returns><see cref="DateOnly"/></returns>
    public static DateOnly ToDateOnly(this DateTime dateTime) 
        => DateOnly.FromDateTime(dateTime);
    /// <summary>
    /// Returns a <see cref="TimeOnly"/> from the given <paramref name="dateTime"/>.
    /// </summary>
    /// <param name="dateTime">Required.</param>
    /// <returns><see cref="TimeOnly"/></returns>
    public static TimeOnly ToTimeOnly(this DateTime dateTime) 
        => TimeOnly.FromDateTime(dateTime);

    public static DateTime ToDateTime(this DateOnly dateOnly) 
        => new (dateOnly.Year, dateOnly.Month, dateOnly.Day);
    public static DateTime ToDateTime(this DateOnly dateOnly, TimeOnly time)
        => new(dateOnly.Year, dateOnly.Month, dateOnly.Day
            , time.Hour, time.Minute, time.Second,time.Millisecond, time.Microsecond);
    public static DateTime ToDateTime(this DateOnly dateOnly, TimeOnly time, DateTimeKind kind)
        => new(dateOnly.Year, dateOnly.Month, dateOnly.Day
            , time.Hour, time.Minute, time.Second, time.Millisecond, time.Microsecond, kind);
    public static DateTime ToDateTime(this DateOnly dateOnly, int hour, int minute, int second)
        => new(dateOnly.Year, dateOnly.Month, dateOnly.Day
            , hour, minute, second);
    public static DateTime ToDateTime(this DateOnly dateOnly, int hour, int minute, int second, DateTimeKind kind)
        => new(dateOnly.Year, dateOnly.Month, dateOnly.Day, hour, minute, second, kind);
#endif
}