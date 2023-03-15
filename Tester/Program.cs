using LinqTools;

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

var result = await Process.RunAsync("lsblkd", "--bytes --output SIZE,NAME,LABEL,MOUNTPOINT,FSTYPE");
result = await Process.RunAsync("lsblk", "--nothing -bytes -output SIZE,NAME,LABEL,MOUNTPOINT,FSTYPE");
result = await Process.RunAsync("lsblk", "--bytes --output SIZE,NAME,LABEL,MOUNTPOINT,FSTYPE");
var test = from n in await Process.RunAsync("lsblk", "--bytes --output SIZE,NAME,LABEL,MOUNTPOINT,FSTYPE")
           let driveLines = n.Split('\n', StringSplitOptions.RemoveEmptyEntries)
           let titles = driveLines[0]
           let positions = new[]
           {
               0,
               titles.IndexOf("NAME"),
               titles.IndexOf("LABEL"),
               titles.IndexOf("MOUNT"),
               titles.IndexOf("FSTYPE")
           }
           select driveLines
                    .Skip(1)
                    .Select(n => CreateRootItem(n, positions))
                    .ToArray();
var test2 = 0;

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
