using LinqTools;

using static LinqTools.Core;

var teststr = "Uwe Riegel".FromNullable();

string getSubstring5(string test)
    => test.Substring(5);

Option<string> getSubstring5Checked(string test)
    => test.Length > 50
        ? test.Substring(5)
        : None;

string ThrowIfNull(string nix)        
{
    if (nix == null)
        throw new NullReferenceException();
    return nix;
}

Option<int> GetMaybeInt(string a)
    => 24;

var res = (from n in teststr
            select getSubstring5(n))
                .GetOrDefault("Nichts");

var res2 = (from n in teststr
            from m in getSubstring5Checked(n)
            where m.Length == 9
            let p = ThrowIfNull(m)
            select p)
                .GetOrDefault("Nichts");

var res3 = (from n in teststr
           from m in getSubstring5Checked(n) 
           from p in GetMaybeInt(m)
           select p)
                .GetOrDefault(99);

var res4 = (from n in teststr
            where n.Length > 10
            select getSubstring5(n))   
                .GetOrDefault("Nichts");

var lastWriteTime = DateTime.Now;
var dateTimeString1 = "";
var dateTimeString2 = "Sat, 18 Mar 2023 10:43:32 GMT";


var ares = (await (from n in teststr
                        .ToAsyncOption()
                    select getSubstring5(n))
                        .ToOption())
            .GetOrDefault("Nichts");

// var ares2 = ((from n in teststr
//                         .ToAsyncOption()
//              select getSubstring5Async(n))
//                     .ToOption())
//                     //.GetOrDefault("Nichts");

var a = 9;

async Task<string> getSubstring5Async(string test)
{
    await Task.Delay(1000);
    return getSubstring5(test);
}


bool IsModified(string dateTimeString)
    => (from n in dateTimeString
                    .WhiteSpaceToNone()
        let m = n.FromString()
        select lastWriteTime > m)
            .GetOrDefault(true);

var test3 = IsModified(dateTimeString1);
var test4 = IsModified(dateTimeString2);
var t = 9;

RootItem CreateRootItem(string driveString, int[] positions)
{
    var mountPoint = GetString(3, 4);
    
    return new(
        GetString(1, 2),
        GetString(2, 3),
        GetString(0, 1)
            .ParseLong()
            .GetOrDefault(0),
        mountPoint,
        mountPoint.Length > 0,
        driveString[(positions[4])..]
            .Trim()
    );

    string GetString(int pos1, int pos2)
        => driveString[positions[pos1]..positions[pos2]].Trim();
}

record RootItem(
    string Name,
    string Description,
    long Size,
    string MountPoint,
    bool IsMounted,
    string DriveType
);

static class Extensions
{
    public static DateTime FromString(this string timeString)
        => Convert.ToDateTime(timeString);

    public static string? WhiteSpaceToNull(this string? str)
        => !string.IsNullOrWhiteSpace(str) 
            ? str
            : null;

    public static Option<string> WhiteSpaceToNone(this string? str)
        => !string.IsNullOrWhiteSpace(str)
            ? str
            : None;

    public static Option<long> ParseLong(this string? str)
        => long.TryParse(str, out var val)
            ? val
            : None;
}