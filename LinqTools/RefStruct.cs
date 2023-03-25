namespace LinqTools;

public class RefStruct<T>
    where T : struct
{
    public static implicit operator T(RefStruct<T> value)
        => value.t;

    public static implicit operator RefStruct<T>(T value)
        => value.ToRef();

    internal RefStruct(T t) => this.t = t;

    readonly T t;
}

public static class RefStructExtensions
{
    public static T GetOrDefault<T>(this RefStruct<T>? value, T defaultValue)
        where T : struct
        => value != null
            ? value
            : defaultValue;

    public static RefStruct<T> ToRef<T>(this T t) 
        where T : struct
        => new(t);
}
