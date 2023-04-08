namespace LinqTools;

public static class NullableExtensions
{
    /// <summary>
    /// If not sure if a value is null although it is a not nullable, you can create a nullable Monad
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static T? AsNullable<T>(this T t)
        where T : class
        => t != null
            ? t
            : null;

    /// <summary>
    /// Get the value of a nullable if it is not null otherwise get a default value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static T GetOrDefault<T>(this T? t, T defaultValue)
        where T : class
        => t != null
            ? t
            : defaultValue;

    /// <summary>
    /// Get the value of a nullable if it is not null otherwise get a default value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static T GetOrDefault<T>(this T? t, T defaultValue)
        where T : struct
        => t.HasValue
            ? t.Value
            : defaultValue;

    /// <summary>
    /// If a value is null, call function 'elseWith' and take that value, otherwise take the first value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="elseWith"></param>
    /// <returns></returns>
    public static T? OrElseWith<T>(this T? t, Func<T?> elseWith)
        where T : class
        => t != null
            ? t
            : elseWith();
}

public static partial class Core
{
    /// <summary>
    /// Create a nullable by calling a function. If an exception occurs the return value is null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static T? ExceptionToNull<T>(Func<T> func)
        where T : notnull
    {
        try
        {
            return func();
        }
        catch
        {
            return default(T?);
        }
    }
}

