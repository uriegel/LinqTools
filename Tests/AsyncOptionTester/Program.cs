using LinqTools;

var teststr = "Uwe Riegel".FromNullable();

var ares = (await (from n in teststr
                        .ToAsyncOption()
                    select getSubstring5(n))
                        .ToOption())
            .GetOrDefault("Nichts");
Console.WriteLine(ares);

var ares2 = (await ((from n in teststr
                        .ToAsyncOption()
                        .SelectAwait(n => getSubstring5Async(n))
                    select n)
                .ToOption()))
                .GetOrDefault("Nichts");
Console.WriteLine(ares2);

string getSubstring5(string test)
    => test.Substring(5);

async Task<string> getSubstring5Async(string test)
{
    await Task.Delay(1000);
    return getSubstring5(test);
}


