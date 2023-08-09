using static LinqTools.Core;

namespace LinqTools;

public readonly struct Option<T> where T : notnull
{
    public bool IsSome { get ; }
    
    public Option()
    {
        Value = default;
        IsSome = false;
    }

    internal Option(T value)
    {
        Value = value;
        IsSome = true;
    }

    public override string ToString() => IsSome ? $"{Value}" : "";

    public static implicit operator Option<T>(NoneType _) => default;
    public static implicit operator Option<T>(T value) => value is null ? None : Some(value);

    public Option<T> Where(Func<T, bool> predicate)
        => IsSome
            ? predicate(Value!) 
                ? Value! 
                : None
            : None;

    public void Match(Action<T> successAction, Action failAction)
    {
        if (IsSome)
            successAction(Value!);
        else
            failAction();
    }

    public TResult Match<TResult>(Func<T, TResult> successFunc, Func<TResult> failFunc)
        => IsSome
            ? successFunc(Value!)
            : failFunc();

    public Option<T> WhenSome(Action<T> sideEffect)
    {
        if (IsSome)
            sideEffect(Value!);
        return this;
    }

    public Option<T> WhenNone(Action sideEffect)
    {
        if (!IsSome)
            sideEffect();
        return this;
    }

    public Option<B> As<B>() where B : class
        => Match(o => (o as B).FromNullable(), () => None);

    T? Value { get; }
}

public static class OptionExtensions
{
    public static Option<T> FromNullable<T>(this T? value)
        where T : notnull
        => value != null ? Some(value) : None;

    public static Option<T> FromNullableStruct<T>(this T? value)
        where T : struct
        => value.HasValue ? Some(value.Value) : None;

    public static T GetOrDefault<T>(this Option<T> option, T defaultValue)
        where T : notnull
        => option.Match(val => val, () => defaultValue);

    public static T? GetOrNull<T>(this Option<T> option)
        where T: class
        => option.Match(val => val, () => default!);

    public static T? GetOrNullable<T>(this Option<T> option)
        where T: struct
        => option.Match(val => val, () => default!);

public static T GetOrDefault<T>(this Option<T> option, Func<T> defaultValue)
        where T : notnull
        => option.Match(val => val, () => defaultValue());

    public static IEnumerable<T> ToEnumerable<T>(this Option<T> option)
        where T : notnull
        => option.IsSome 
            ? new T[] { option.ThrowOnNone() }
            : Enumerable.Empty<T>();

    public static T ThrowOnNone<T>(this Option<T> option, Func<Exception> throwException)
        where T : notnull
        => option.Match(val => val, () => throw throwException());

    public static T ThrowOnNone<T>(this Option<T> option)
        where T : notnull
        => option.Match(val => val, () => throw new KeyNotFoundException());

    public static Option<T> Or<T>(this Option<T> option, Func<Option<T>> getAlternative)
        where T : notnull
        => option.IsSome 
            ? option
            : getAlternative();

    public static Option<R> Select<T, R>(this Option<T> opt, Func<T, R> func)
        where R : notnull
        where T : notnull
        => opt.Match(
            some => Some(func(some)),
            ()   => None
        );

    public static Option<T> Where<T>(this Option<T> option, Func<T, bool> predicate)
        where T : notnull
        => option.Match(
            t => predicate(t) ? option : None,
            () => None
        );

    // public static Option<Unit> ForEach<T>(this Option<T> opt, Action<T> action)
    //     where T : notnull
    //     => opt.Map(action.ToUnit());

    // public static Option<Unit> ForEach<T>(this Option<T> opt, Func<T, Unit> action)
    //     where T : notnull
    //     => opt.Map(action);

    public static Option<R> SelectMany<T, R>(this Option<T> opt, Func<T, Option<R>> func)
        where R : notnull
        where T : notnull
        => opt.Match(
            t => func(t).Match(
                Some,
                ()   => None
            ),
            ()       => None
        );

    public static Option<RR> SelectMany<T, R, RR>(this Option<T> opt, Func<T, Option<R>> bind, Func<T, R, RR> project)
        where RR : notnull
        where R : notnull
        where T : notnull
        => opt.Match(
            t => bind(t).Match(
                r => Some(project(t, r)),
                () => None
            ),
            () => None
        );

    //public static Option<R> SelectMany<T, R>(this Option<T> opt, Func<T, Option<R>> func) => Bind(opt, func);
}

public struct NoneType { }

public static class NoneTypeExtensions
{
    public static NoneType SideEffect(this NoneType none, Action sideEffect)
    {
        sideEffect();
        return none;
    }
}

public static partial class Core
{
    public static Option<T> Some<T>(T value)
        where T : notnull
        => new(value);
    public static NoneType None => default;

    public static Option<(U, V)> OptionFromTwoOptions<U, V>(Option<U> option1, Option<V> option2)
        where U : notnull
        where V : notnull
        => (option1, option2) switch
        {
            (Option<U> { IsSome: true }, Option<V> { IsSome: true }) => Some((option1.ThrowOnNone(), option2.ThrowOnNone())),
            _ => None
        };
}
