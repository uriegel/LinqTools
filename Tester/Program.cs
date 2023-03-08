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
           from p in ThrowIfNull(m)
           select p).GetOrDefault("Nichts");

var res3 = (from n in teststr
           from m in getFirstStringChecked(n) 
           from p in ThrowIfNull(m)
           select GetNullableInt(p)).GetOrDefault(99);


