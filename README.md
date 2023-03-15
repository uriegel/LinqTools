# LinqTools

Extensions for LINQ

## Overview

* Extension functions to make Nullable a Monad/Functor
* Extension functions to make Async Function a Monad/Functor
* Ohter Tools usable for fluent syntax

## Nullable Extensions

There are Extension Functions for nullables to make them Monads/Functors:
* Select
* SelectMany

If you are not sure, if an object is null although the compiler says no, you can create a Nullable from an apparently not Nullable with
```
static T? AsNullable<T>(this T t)

``` 
With the extension functions you can work with nullables as if they were not nullables:
``` 
var res = (from n in teststr
           from m in GetFirstStringChecked(n) 
           where m.Length == 9
           from p in ThrowIfNull(m)
           select p)
                .GetOrDefault("Nothing");
``` 
As soon as a function returns null, all further processing is skipped.

## Result Monad/Functor

There is a Result Monad/Functor, containing either an Ok value or an Error value. You can transform
Results with LINQ queries like:

```
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
```
