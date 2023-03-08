namespace LinqTools;

public static class GenericNullable
{
    public static R? Select<T, R>(this T? opt, Func<T, R> func) 
        where T : class
        => opt != null
        ? func(opt)
        : default(R?);

    public static R? Select<T, R>(this T? opt, Func<T, R> func) 
        where T : struct
        => opt.HasValue
        ? func(opt.Value)
        : default(R?);

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

    public static RR? SelectMany<T, R, RR>(this T? t, Func<T, R?> bind, Func<T, R, RR> project)
        where R : struct
        where T : class
        => t != null
            ? bind(t) switch 
            {
                null => default(RR?),
                R r => project(t, r)
            }
            : default(RR?);

    public static RR? SelectMany<T, R, RR>(this T? t, Func<T, R?> bind, Func<T, R, RR> project)
        where R : class
        where T : struct
        => t.HasValue
            ? bind(t.Value) switch 
            {
                null => default(RR?),
                R r => project(t.Value, r)
            }
            : default(RR?);

    public static RR? SelectMany<T, R, RR>(this T? t, Func<T, R?> bind, Func<T, R, RR> project)
        where R : struct
        where T : struct
        => t.HasValue
            ? bind(t.Value) switch 
            {
                null => default(RR?),
                R r => project(t.Value, r)
            }
            : default(RR?);

    public static T GetOrDefault<T>(this T? t, T defaultValue)
        where T : class
        => t != null
            ? t
            : defaultValue;

    public static T GetOrDefault<T>(this T? t, T defaultValue)
        where T : struct
        => t.HasValue
            ? t.Value
            : defaultValue;

    // public static bool Where<T>(this T? opt, Action<T> action)
    //     where T : struct
    // {
    //     if (opt.HasValue)
    //         action(opt.Value);
    // }

    public static void ForEach<T>(this T? opt, Action<T> action)
        where T : class
    {
        if (opt != null)
            action(opt);
    }

    public static void ForEach<T>(this T? opt, Action<T> action)
        where T : struct
    {
        if (opt.HasValue)
            action(opt.Value);
    }
}