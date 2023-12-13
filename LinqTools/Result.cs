namespace LinqTools;

using static Core;

/// <summary>
/// Functional Type (Discriminated Union) containing either a T Ok type or a TE Error type
/// </summary>
/// <typeparam name="T">Typeof OK</typeparam>
/// <typeparam name="TE">Typeof Error</typeparam>
public readonly struct Result<T, TE> 
    where T : notnull
    where TE : notnull
{
    internal Result(T value)
    {
        IsOK = true;
        OkValue = value;
        EValue = default;
    }

    internal Result(TE value)
    {
        IsOK = false;
        OkValue = default;
        EValue = value;
    }

    public static implicit operator Result<T, TE>(T value)
        => Ok<T, TE>(value);

    public bool IsOK { get; }  

    /// <summary>
    /// Pattern matching for Result 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="successFunc"></param>
    /// <param name="failFunc"></param>
    /// <returns></returns>
    public TResult Match<TResult>(Func<T, TResult> successFunc, Func<TE, TResult> failFunc)
        => IsOK
        ? successFunc(OkValue!)
        : failFunc(EValue!);

    /// <summary>
    /// Pattern matching for Result 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="successFunc"></param>
    /// <param name="failFunc"></param>
    /// <returns></returns>
    public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> successFunc, Func<TE, TResult> failFunc)
        => IsOK
        ? await successFunc(OkValue!)
        : failFunc(EValue!);

    /// <summary>
    /// Pattern matching for Result 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="successFunc"></param>
    /// <param name="failFunc"></param>
    /// <returns></returns>
    public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> successFunc, Func<TE, Task<TResult>> failFunc)
        => IsOK
        ? await successFunc(OkValue!)
        : await failFunc(EValue!);

    /// <summary>
    /// Pattern matching for Result 
    /// </summary>
    /// <param name="successAction"></param>
    /// <param name="failAction"></param>
    public void Match(Action<T> successAction, Action<TE> failAction)
    {
        if (IsOK)
            successAction(OkValue!);
        else
            failAction(EValue!);
    }

    internal static async Task<TResult> MatchInternalAsync<TResult>(Task<Result<T, TE>> result,
        Func<T, TResult> successFunc, Func<TE, TResult> failFunc)
    {
        var awaitedsResult = await result;
        if (awaitedsResult.IsOK)
            return successFunc(awaitedsResult.OkValue!);
        else
            return failFunc(awaitedsResult.EValue!);
    }

    internal static async Task<TResult> MatchInternalAsync<TResult>(Task<Result<T, TE>> result,
        Func<T, Task<TResult>> successFunc, Func<TE, TResult> failFunc)
    {
        var awaitedsResult = await result;
        if (awaitedsResult.IsOK)
            return await successFunc(awaitedsResult.OkValue!);
        else
            return failFunc(awaitedsResult.EValue!);
    }

    internal static async Task<TResult> MatchInternalAsync<TResult>(Task<Result<T, TE>> result,
        Func<T, Task<TResult>> successFunc, Func<TE, Task<TResult>> failFunc)
    {
        var awaitedsResult = await result;
        if (awaitedsResult.IsOK)
            return await successFunc(awaitedsResult.OkValue!);
        else
            return await failFunc(awaitedsResult.EValue!);
    }

    T? OkValue { get; }
    TE? EValue { get; }
}

public static class ResultExtensions
{
    /// <summary>
    /// Pattern matching for Result 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <param name="result"></param>
    /// <param name="successFunc"></param>
    /// <param name="failFunc"></param>
    /// <returns></returns>
    public static Task<TResult> MatchAsync<TResult, T, TE>(this Task<Result<T, TE>> result,
        Func<T, TResult> successFunc, Func<TE, TResult> failFunc)
        where T : notnull
        where TE : notnull
        => Result<T, TE>.MatchInternalAsync(result, successFunc, failFunc);

    /// <summary>
    /// Pattern matching for Result 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <param name="result"></param>
    /// <param name="successFunc"></param>
    /// <param name="failFunc"></param>
    /// <returns></returns>
    public static Task<TResult> MatchAsync<TResult, T, TE>(this Task<Result<T, TE>> result,
        Func<T, Task<TResult>> successFunc, Func<TE, TResult> failFunc)
        where T : notnull
        where TE : notnull
        => Result<T, TE>.MatchInternalAsync(result, successFunc, failFunc);

    /// <summary>
    /// Pattern matching for Result 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <param name="result"></param>
    /// <param name="successFunc"></param>
    /// <param name="failFunc"></param>
    /// <returns></returns>
    public static Task<TResult> MatchAsync<TResult, T, TE>(this Task<Result<T, TE>> result,
        Func<T, Task<TResult>> successFunc, Func<TE, Task<TResult>> failFunc)
        where T : notnull
        where TE : notnull
        => Result<T, TE>.MatchInternalAsync(result, successFunc, failFunc);

    /// <summary>
    /// Get the Ok value or throws the Error exception
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static T GetOrThrow<T, TE>(this Result<T, TE> result)
        where T : notnull
        where TE : Exception
        => result.Match(ok => ok, error => throw error);

    /// <summary>
    /// Transforms the OK value (if present) to another type in a LINQ query 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static Task<T> GetOrThrowAsync<T, TE>(this Task<Result<T, TE>> result)
        where T : notnull
        where TE : Exception
        => result.MatchAsync(ok => ok, error => throw error);

    /// <summary>
    /// Transforms the OK value (if present) to another type in a LINQ query 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="result"></param>
    /// <param name="selector">Function (selector) transforming T to R</param>
    /// <returns></returns>
    public static Result<R, TE> Select<T, TE, R>(this Result<T, TE> result, Func<T, R> selector)
        where T : notnull
        where TE : notnull
        where R : notnull
        => result.Match(
            t => new Result<R, TE>(selector(t)),
            e => new Result<R, TE>(e)
        );

    /// <summary>
    /// Transforms the OK value (if present) to another type in a LINQ query 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="result"></param>
    /// <param name="selector">Function (selector) transforming T to R</param>
    /// <returns></returns>
    public static async Task<Result<R, TE>> Select<T, TE, R>(this Result<T, TE> result, Func<T, Task<R>> selector)
        where T : notnull
        where TE : notnull
        where R : notnull
        => await result.MatchAsync(
            async t => new Result<R, TE>(await selector(t)),
            e => new Result<R, TE>(e)
        );

    /// <summary>
    /// Transforms the OK value (if present) to another type in a LINQ query 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="result"></param>
    /// <param name="selector">Function (selector) transforming T to R</param>
    /// <returns></returns>
    public static async Task<Result<R, TE>> Select<T, TE, R>(this Task<Result<T, TE>> result, Func<T, R> selector)
    where T : notnull
    where TE : notnull
    where R : notnull
    => (await result).Match(
        t => new Result<R, TE>(selector(t)),
        e => new Result<R, TE>(e)
    );

    /// <summary>
    /// Transforms the OK value (if present) to another type in a LINQ query 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="result"></param>
    /// <param name="selector">Function (selector) transforming T to R</param>
    /// <returns></returns>
    public static async Task<Result<R, TE>> Select<T, TE, R>(this Task<Result<T, TE>> result, Func<T, Task<R>> selector)
        where T : notnull
        where TE : notnull
        where R : notnull
        => await (await result).MatchAsync(
            async t => new Result<R, TE>(await selector(t)),
            e => new Result<R, TE>(e)
        );

    //public static Result<T, TE> Where<T, TE>(this Result<T, TE> result, Func<T, bool> predicate)
    //    where T : notnull
    //    where TE : notnull
    //    => result.Match(
    //        t => predicate(t) ? result : Error<T, TE>.New(new Exception()),
    //        e => Error<T, TE>.New(e)
    //    );

    //public static async Task<Result<T>> Where<T>(this Task<Result<T>> result, Func<T, bool> predicate)
    //    where T : notnull
    //    => await (await result).MatchAsync(
    //        async t => predicate(t) ? await result : Error<T>.New(new Exception()),
    //        e => Error<T>.New(e)
    //    );

    // public static Result<Unit, TE> ForEach<T, TE>(this Result<T, TE> result, Action<T> action)
    //     where T : notnull
    //     where TE : notnull
    //     => result.Map(action.ToUnit());

    
    /// <summary>
    /// Transforms the OK value (if present) to Result containing another type.
    /// </summary>
    /// <typeparam name="T">Source type</typeparam>
    /// <typeparam name="TE">Exception type</typeparam>
    /// <typeparam name="TResult">Target Type</typeparam>
    /// <param name="result"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static Result<TResult, TE> SelectMany<T, TE, TResult>(this Result<T, TE> result,
            Func<T, Result<TResult, TE>> selector)
        where T : notnull
        where TE : notnull
        where TResult : notnull
        => result.Match(
            t => selector(t),
            e => new Result<TResult, TE>(e)
        );

    /// <summary>
    /// Transforms the OK value (if present) to Result containing another type. For use in a LINQ query 
    /// </summary>
    /// <typeparam name="T">Source type</typeparam>
    /// <typeparam name="TE">Exception type</typeparam>
    /// <typeparam name="R">Intermediate type</typeparam>
    /// <typeparam name="TResult">Target Type</typeparam>
    /// <param name="result"></param>
    /// <param name="selector"></param>
    /// <param name="resultSelector"></param>
    /// <returns></returns>
    public static Result<TResult, TE> SelectMany<T, TE, R, TResult>(this Result<T, TE> result,
            Func<T, Result<R, TE>> selector, Func<T, R, TResult> resultSelector)
        where T : notnull
        where TE : notnull
        where R : notnull
        where TResult : notnull
        => result.Match(
            t => selector(t).Match(
                r => new Result<TResult, TE>(resultSelector(t, r)),
                e => new Result<TResult, TE>(e)
            ),
            e => new Result<TResult, TE>(e)
        );

    /// <summary>
    /// Transforms the OK value (if present) to Result containing another type. For use in a LINQ query 
    /// </summary>
    /// <typeparam name="T">Source type</typeparam>
    /// <typeparam name="TE">Exception type</typeparam>
    /// <typeparam name="R">Intermediate type</typeparam>
    /// <typeparam name="TResult">Target Type</typeparam>
    /// <param name="result"></param>
    /// <param name="selector"></param>
    /// <param name="resultSelector"></param>
    /// <returns></returns>
    public static async Task<Result<TResult, TE>> SelectMany<T, TE, R, TResult>(this Task<Result<T, TE>> result,
            Func<T, Task<Result<R, TE>>> selector, Func<T, R, Task<TResult>> resultSelector)
        where T : notnull
        where TE : notnull
        where R : notnull
        where TResult: notnull
        => await (await result).MatchAsync(
            async t => await (await selector(t)).MatchAsync(
                async r => new Result<TResult, TE>(await resultSelector(t, r)),
                e => new Result<TResult, TE>(e)
            ),
            e => new Result<TResult, TE>(e)
        );

    /// <summary>
    /// Transforms the OK value (if present) to Result containing another type. For use in a LINQ query 
    /// </summary>
    /// <typeparam name="T">Source type</typeparam>
    /// <typeparam name="TE">Exception type</typeparam>
    /// <typeparam name="R">Intermediate type</typeparam>
    /// <typeparam name="TResult">Target Type</typeparam>
    /// <param name="result"></param>
    /// <param name="selector"></param>
    /// <param name="resultSelector"></param>
    /// <returns></returns>
    public static async Task<Result<TResult, TE>> SelectMany<T, TE, R, TResult>(this Task<Result<T, TE>> result,
            Func<T, Task<Result<R, TE>>> selector, Func<T, R, TResult> resultSelector)
        where T : notnull
        where TE : notnull
        where R : notnull
        where TResult : notnull
        => await (await result).MatchAsync(
            async t => (await selector(t)).Match(
                r => new Result<TResult, TE>(resultSelector(t, r)),
                e => new Result<TResult, TE>(e)
            ),
            e => new Result<TResult, TE>(e)
        );

    /// <summary>
    /// Transforms the Exception type to another rype
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <typeparam name="TER"></typeparam>
    /// <param name="result"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static Result<T, TER> SelectException<T, TE, TER>(this Result<T, TE> result, Func<TE, TER> selector)
        where T : notnull
        where TE : notnull
        where TER : notnull
        => result.Match(
            t => t,
            e => new Result<T, TER>(selector(e))
        );
    
    /// <summary>
    /// Transforms the Exception type to another rype
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <typeparam name="TER"></typeparam>
    /// <param name="result"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static Task<Result<T, TER>> SelectException<T, TE, TER>(this Task<Result<T, TE>> result, Func<TE, TER> selector)
        where T : notnull
        where TE : notnull
        where TER : notnull
        => result.MatchAsync(
            t => t,
            e => new Result<T, TER>(selector(e))
        );

    /// <summary>
    /// Gets the Ok value, or get a default value instead the Error
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <param name="result"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static T GetOrDefault<T, TE>(this Result<T, TE> result, T defaultValue)
        where T : notnull
        where TE : notnull
        => result.Match(val => val, e => defaultValue);

    /// <summary>
    /// Gets the Ok value, or get a default value instead the Error
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <param name="result"></param>
    /// <param name="getExceptionValue">Function retrieving result from exception value</param>
    /// <returns></returns>
    public static T Get<T, TE>(this Result<T, TE> result, Func<TE, T> getExceptionValue)
        where T : notnull
        where TE : notnull
        => result.Match(val => val, getExceptionValue);

    /// <summary>
    /// Elevates a Result to a Task&lt;Result&gt; monad
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static Task<Result<T, TE>> ToAsyncResult<T, TE>(this Result<T, TE> result)
        where T : notnull
        where TE : notnull
        => Task.FromResult(result);
}

public static partial class Core
{
    /// <summary>
    /// Creating a Result with an OK value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Result<T, TE> Ok<T, TE>(T value) 
        where T : notnull
        where TE : notnull 
        => new(value);

    /// <summary>
    /// Creating a Result with an Error value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <param name="e"></param>
    /// <returns></returns>
    public static Result<T, TE> Error<T, TE>(TE e)
        where T : notnull
        where TE : notnull
        => new(e);

    /// <summary>
    /// Runs code and returns a Result containing the result, or if it throws an exception the exception as Error
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <param name="func"></param>
    /// <param name="onException"></param>
    /// <returns></returns>
    public static Result<T, TE> Try<T, TE>(Func<T> func, Func<Exception, TE> onException)
        where T : notnull
        where TE : notnull
    {
        try
        {
            return Core.Ok<T, TE>(func());
        }
        catch (Exception ex)
        {
            return Core.Error<T, TE>(onException(ex));
        }
    }

    /// <summary>
    /// Runs code and returns 'Nothing', or if it throws an exception the exception as Error
    /// </summary>
    /// <typeparam name="TE"></typeparam>
    /// <param name="action"></param>
    /// <param name="onException"></param>
    /// <returns></returns>
    public static Result<Nothing, TE> Try<TE>(Action action, Func<Exception, TE> onException)
        where TE : notnull
    {
        try
        {
            action();
            return 0.ToNothing();
        }
        catch (Exception ex)
        {
            return Core.Error<Nothing, TE>(onException(ex));
        }
    }

    /// <summary>
    /// Runs code and returns a Result containing the result, or if it throws an exception the exception as Error
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TE"></typeparam>
    /// <param name="func"></param>
    /// <param name="onException"></param>
    /// <returns></returns>
    public static async Task<Result<T, TE>> TryAsync<T, TE>(Func<Task<T>> func, Func<Exception, TE> onException)
        where T : notnull
        where TE : notnull
    {
        try
        {
            return Core.Ok<T, TE>(await func());
        }
        catch (Exception ex)
        {
            return Core.Error<T, TE>(onException(ex));
        }
    }

    /// <summary>
    /// Runs async code and returns 'Nothing' containing the result, or if it throws an exception the exception as Error
    /// </summary>
    /// <typeparam name="TE"></typeparam>
    /// <param name="func"></param>
    /// <param name="onException"></param>
    /// <returns></returns>
    public static async Task<Result<Nothing, TE>> TryAsync<TE>(Func<Task> func, Func<Exception, TE> onException)
        where TE : notnull
    {
        try
        {
            await func();
            return 0.ToNothing();
        }
        catch (Exception ex)
        {
            return Core.Error<Nothing, TE>(onException(ex));
        }
    }
}
