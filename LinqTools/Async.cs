namespace LinqTools.Async
{
    public static class AsyncTask
    {
        public static async Task<TResult> Map<T, TResult>(this Task<T> task, Func<T, TResult> func)
            => func(await task);

        public static async Task<TResult> Bind<T, TResult>(this Task<T> task, Func<T, Task<TResult>> func)
            => await func(await task);

        public static async Task<TResult> Select<T, TResult>(this Task<T> task, Func<T, TResult> func)
            => func(await task);

        public static async Task<TResult> SelectMany<T, R, TResult>
                (this Task<T> task, Func<T, Task<R>> bind, Func<T, R, TResult> project)
        {
            var t = await task;
            return project(t, await bind(t));
        }

        //public static async Task<TResult> SelectMany<T, R, TResult>(this Task<T> task, Func<T, Task<R>> bind, Func<T, R, TResult> project)
        //    async r => await (await bind(project(await task)), r);
        //);
    }
}

namespace LinqTools
{
    public static partial class Core
    {
        public static Task<T> Async<T>(T t) => Task.FromResult(t);

        // public static Task RunAsync(Action action)
        // {
        //     action();
        //     return Task.FromResult(Unit.Value);
        // }
    }
}

