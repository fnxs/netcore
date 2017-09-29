using System;

namespace FunctionalExtensions.Maybe
{
    public abstract class Maybe<T> { }

    public class Nothing<T> : Maybe<T> { }

    public class Just<T> : Maybe<T>
    {
        public Just(T value) => Value = value;

        public T Value { get; }
    }

    public static class MaybeExtensions
    {
        public static Maybe<T> Return<T>(this T value) => new Just<T>(value);

        public static Maybe<TTo> Bind<TFrom, TTo>(this Maybe<TFrom> from, Func<TFrom, Maybe<TTo>> f)
            => from is Just<TFrom> just
                ? f(just.Value)
                : new Nothing<TTo>();

        public static Maybe<TTo> Compose<TFrom, TTo>(this Maybe<TFrom> from, Maybe<TTo> to)
            => from is Just<TFrom>
                ? to
                : new Nothing<TTo>();
    }
}