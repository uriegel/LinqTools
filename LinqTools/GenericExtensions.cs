namespace LinqTools;

public static class GenericExtensions
{
    /// <summary>
    /// This SideEffect is useful if you want to apply some side effect to a value in a LINQ expression,
    /// such as logging the value. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static T SideEffect<T>(this T t, Action<T> action)
    {
        action(t);
        return t;
    }

    /// <summary>
    /// This SideEffect is useful if you want to apply some side effect to a value in a LINQ expression,
    /// such as logging the value. The value is waitable and will be awaited to apply the side effect
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="sideEffect"></param>
    /// <returns></returns>
    public static async Task<T> SideEffect<T>(this Task<T> value, Action<T> sideEffect)
    {
        var val = await value;
        sideEffect(val);
        return val;
    }

    /// <summary>
    /// This SideEffect is useful if you want to apply some awaitable side effect to a value in a LINQ expression,
    /// such as logging the value. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="text"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static async Task<T> SideEffect<T>(this T text, Func<T, Task> selector)
    {
        await selector(text);
        return text;
    }

    /// <summary>
    /// If you need to an awaitable value, and you have only a normal value, you can create an 
    /// Async monad with this extension function
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Task<T> ToAsync<T>(this T t) => Task.FromResult(t);

    /// <summary>
    /// If you want to ignore some function result, you can apply this method. It returns void.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_"></param>
    public static void ToVoid<T>(this T _) { }
}
