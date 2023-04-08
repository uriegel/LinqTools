using LinqTools;
using LinqTools.Nullable;

String? teststr = "Uwe Riegel";

string getFirstString(string test)
    => test.Substring(5);

string? getFirstStringChecked(string test)
    => test.Length > 50
        ? test.Substring(5)
        : null;

string ThrowIfNull(string nix)        
{
    if (nix == null)
        throw new NullReferenceException();
    return nix;
}

int? GetNullableInt(string a)
    => 24;

var res = (from n in teststr
          select getFirstString(n)).GetOrDefault("Nichts");

var res2 = (from n in teststr
           from m in getFirstStringChecked(n) 
           where m.Length == 9
           from p in ThrowIfNull(m)
           select p).GetOrDefault("Nichts");

var res3 = (from n in teststr
           from m in getFirstStringChecked(n) 
           from p in ThrowIfNull(m)
           select GetNullableInt(p)).GetOrDefault(99);

var res4 = (from n in teststr
          where n.Length > 10
          select getFirstString(n)).GetOrDefault("Nichts");

var lastWriteTime = DateTime.Now;
var dateTimeString1 = "";
var dateTimeString2 = "Sat, 18 Mar 2023 10:43:32 GMT";

bool IsModified(string dateTimeString)
    => (from n in dateTimeString
                    .WhiteSpaceToNull()
                    ?.FromString()
                    .ToRef()
        let r = lastWriteTime > n
        select r.ToRef())
            .GetOrDefault(true);

var test3 = IsModified(dateTimeString1);
var test4 = IsModified(dateTimeString2);

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
        => string.IsNullOrWhiteSpace(str) ? null : str;

    public static long? ParseLong(this string? str)
        => long.TryParse(str, out var val)
            ? val
            : null;
}