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