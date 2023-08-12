namespace LinqTools;

public static class ChooseExtensions
{
    public static Option<TResult> Choose<TResult, T>(this T t, params SwitchType<T, TResult>[] switches)
        where TResult : notnull
        => switches
            .FirstOrNone(s => s.Predicate(t))
            .Select(s => s.Selector(t));

    public record SwitchType<T, TResult>(Predicate<T> Predicate, Func<T, TResult> Selector)
    {
        public static SwitchType<T, TResult> Switch(Predicate<T> predicate, Func<T, TResult> selector)
            => new(predicate, selector);
        public static SwitchType<T, TResult> Default(Func<T, TResult> selector)
            => new(_ => true, selector);
    };
}
