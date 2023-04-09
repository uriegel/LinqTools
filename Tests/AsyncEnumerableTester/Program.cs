
// Async foreach
await foreach (var n in GetAsyncInts())
    Console.WriteLine($"Next number: {n}");

// Async foreach with Extensions
//...

// Select
var numberStrings = from n in GetAsyncInts()
                    select n.StringFromNumber();
await foreach (var n in numberStrings)
    Console.WriteLine($"Next number: {n}");

// SelectMany
var numberStrings2 = 
    from n in GetAsyncInts()
    from m in n.StringsFromNumber()
               .ToAsyncEnumerable()
    select m;
await foreach (var n in numberStrings2)
    Console.WriteLine($"Next number: {n}");

var numberStrings3 = 
    from n in GetAsyncInts()
    from m in n.StringsFromNumberAsync()
    select m;
await foreach (var n in numberStrings3)
    Console.WriteLine($"Next async number: {n}");

// SelectAsync
var numberStringsAsync =
    from n in GetAsyncInts()
                .SelectAwait(async n => await n.StringFromNumberAsync())
    select n;
await foreach (var n in numberStringsAsync)
    Console.WriteLine($"Next async number: {n}");

var numberStringsAsync2 =
    from n in GetAsyncInts()
                .SelectAwait(async n => await n.StringFromNumberAsync())
    select n;
await foreach (var n in numberStringsAsync)
    Console.WriteLine($"Next async number: {n}");

static async IAsyncEnumerable<int> GetAsyncInts()
{
    foreach (var i in Enumerable.Range(1, 10))
    {
        await Task.Delay(400);
        yield return i;
    }
}

static class Extensions
{
    public static string StringFromNumber(this int n)
        => $"Number: {n}";

    public static string[] StringsFromNumber(this int n)
        => new[] { $"Number: {n}-1", $"Number: {n}-2" };

    public static async IAsyncEnumerable<string> StringsFromNumberAsync(this int n)
    {
        await Task.Delay(1000);
        yield return $"Async number: {n}-1";
        await Task.Delay(1000);
        yield return $"Async number: {n}-2";
    }

    public static async Task<string> StringFromNumberAsync(this int n)
    {
        await Task.Delay(400);
        return $"Async Number: {n}";
    }
}