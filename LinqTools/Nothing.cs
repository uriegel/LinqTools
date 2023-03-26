namespace LinqTools;

/// <summary>
/// Functional void
/// </summary>
public struct Nothing { }

public static class NothingExtensions
{
    /// <summary>
    /// Converts any type to Nothing
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_"></param>
    /// <returns></returns>
    public static Nothing ToNothing<T>(this T _)
        => empty;

    static Nothing empty = new Nothing();
}

public static partial class Core
{
    /// <summary>
    /// Runs an action and returns Nothing
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Nothing ToNothing(Action action)
    {
        action();
        return 0.ToNothing();
    }

    /// <summary>
    /// Runs an async action and returns Nothing
    /// </summary>
    /// <param name="asyncAction"></param>
    /// <returns></returns>
    public static async Task<Nothing> ToNothing(Func<Task> asyncAction)
    {
        await asyncAction();
        return 0.ToNothing();
    }
}