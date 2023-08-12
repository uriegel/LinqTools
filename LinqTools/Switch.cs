namespace LinqTools;

static class ChooseExtensions
{
    public static Option<TResult> Choose<TResult, T>(this T t, params SwitchType<TResult, T>[] switches)
        where TResult : notnull
        => switches
            .FirstOrNone(s => s.Predicate(t))
            .Select(s => s.Selector(t));

    public record SwitchType<TResult, T>(Predicate<T> Predicate, Func<T, TResult> Selector)
    {
        public static SwitchType<TResult, T> Switch(Predicate<T> predicate, Func<T, TResult> selector)
            => new(predicate, selector);
        public static SwitchType<TResult, T> Default(Func<T, TResult> selector)
            => new(_ => true, selector);
    };
}
