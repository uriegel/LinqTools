namespace LinqTools.Nullable;

[Obsolete("Use Option<T>", true)]
public static class GenericNullable
{
    /// <summary>
    /// Use nullable in LINQ query syntax
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="opt"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static R? Select<T, R>(this T? opt, Func<T, R> func) 
        where T : class
        where R : class
        => opt != null
        ? func(opt)
        : default(R?);

    /// <summary>
    /// Use nullable in LINQ query syntax
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="opt"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static R? Select<T, R>(this T? opt, Func<T, R> func) 
        where T : struct
        where R : class
        => opt.HasValue
        ? func(opt.Value)
        : default(R?);

    /// <summary>
    /// Use nullable in LINQ query syntax
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <typeparam name="RR"></typeparam>
    /// <param name="t"></param>
    /// <param name="bind"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    public static RR? SelectMany<T, R, RR>(this T? t, Func<T, R?> bind, Func<T, R, RR> project)
        where R : class
        where T : class
        => t != null
            ? bind(t) switch 
            {
                null => default(RR?),
                R r => project(t, r)
            }
            : default(RR?);

    /// <summary>
    /// Use nullable in LINQ query syntax
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <typeparam name="RR"></typeparam>
    /// <param name="t"></param>
    /// <param name="bind"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    public static RR? SelectMany<T, R, RR>(this T? t, Func<T, R?> bind, Func<T, R, RR> project)
        where R : struct
        where T : class
        where RR : class
        => t != null
            ? bind(t) switch 
            {
                null => default(RR?),
                R r => project(t, r)
            }
            : default(RR?);

    /// <summary>
    /// Use nullable in LINQ query syntax
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <typeparam name="RR"></typeparam>
    /// <param name="t"></param>
    /// <param name="bind"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    public static RR? SelectMany<T, R, RR>(this T? t, Func<T, R?> bind, Func<T, R, RR> project)
        where R : class
        where T : struct
        where RR : class
        => t.HasValue
            ? bind(t.Value) switch 
            {
                null => default(RR?),
                R r => project(t.Value, r)
            }
            : default(RR?);

    /// <summary>
    /// Use nullable in LINQ query syntax
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <typeparam name="RR"></typeparam>
    /// <param name="t"></param>
    /// <param name="bind"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    public static RR? SelectMany<T, R, RR>(this T? t, Func<T, R?> bind, Func<T, R, RR> project)
        where R : struct
        where T : struct
        where RR : class
        => t.HasValue
            ? bind(t.Value) switch 
            {
                null => default(RR?),
                R r => project(t.Value, r)
            }
            : default(RR?);

    /// <summary>
    /// Filter an nullable. If filter condition is false, return null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="opt"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static T? Where<T>(this T? opt, Func<T, bool> predicate)
        where T : class
        => opt != null && predicate(opt)
            ? opt
            : null;

    /// <summary>
    /// Filter an nullable. If filter condition is false, return null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="opt"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static T? Where<T>(this T? opt, Func<T, bool> predicate)
        where T : struct
        => opt.HasValue && predicate(opt.Value)
            ? opt
            : null;

    /// <summary>
    /// Iterate through an nullable, that is call action only if value is != null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="opt"></param>
    /// <param name="action"></param>
    public static void ForEach<T>(this T? opt, Action<T> action)
        where T : class
    {
        if (opt != null)
            action(opt);
    }

    /// <summary>
    /// Iterate through an nullable, that is call action only if value is != null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="opt"></param>
    /// <param name="action"></param>
    public static void ForEach<T>(this T? opt, Action<T> action)
        where T : struct
    {
        if (opt.HasValue)
            action(opt.Value);
    }
}

