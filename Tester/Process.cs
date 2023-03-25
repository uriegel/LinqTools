using LinqTools;
using LinqTools.Async;

using static Core;
using static LinqTools.Core;

static class Process
{
    public class Exception : System.Exception
    {
        public string? Error { get; }
        public int? ExitCode{ get; }
        public new System.Exception? InnerException { get; }

        internal Exception(string? error, int code)
        {
            Error = error;
            ExitCode = code;
        }

        internal Exception(System.Exception e)
            => InnerException = e;
    }

    public static Task<Result<string, Exception>> RunAsync(string fileName, string args)
        => Try(async () =>
            {
                var proc = await new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        FileName = fileName,
                        Arguments = args,
                        CreateNoWindow = true
                    }
                }
                    .SideEffect(p => p.Start())
                    .SideEffect(p => p.WaitForExitAsync());
                return 
                    (from n in await proc.StandardOutput.ReadToEndAsync()
                    from m in n.WhiteSpaceToNull()
                    select m)
                        ?? Error<string, Exception>(new Exception((await proc.StandardError.ReadToEndAsync()).WhiteSpaceToNull(), proc.ExitCode));
            }
        , e => Error<string, Exception>(new Exception(e)));
}

static class test
{
    public static string? WhiteSpaceToNull(this string? str)
        => string.IsNullOrWhiteSpace(str) ? null : str;

    public static long? ParseLong(this string? str)
        => long.TryParse(str, out var val)
            ? val
            : null;
}