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

    public static class Extensions
    {
        // ReturnMaybe :: a -> Maybe a
        public static Maybe<T> ReturnMaybe<T>(this T value) 
            => new Just<T>(value);
        
        // ReturnMaybe :: M a -> M a
        public static Maybe<T> ReturnMaybe<T>(this Maybe<T> value)
            => value;
        
        // ReturnMaybe :: Nothing a -> M a
        public static Maybe<T> ReturnMaybe<T>(this Nothing<T> nothing)
            => nothing;
        
        // ReturnMaybe :: Just a -> M a
        public static Maybe<T> ReturnMaybe<T>(this Just<T> just)
            => just;

        // Bind :: M a -> (a -> M b) -> M b 
        public static Maybe<TTo> Bind<TFrom, TTo>(this Maybe<TFrom> from, Func<TFrom, Maybe<TTo>> f)
            => from is Just<TFrom> just
                ? f(just.Value)
                : new Nothing<TTo>();

        // Compose :: M a -> M b -> M b
        public static Maybe<TTo> Compose<TFrom, TTo>(this Maybe<TFrom> from, Maybe<TTo> to)
            => from.Bind(ignored => to);
        
        public static Func<Maybe<TFrom>, Maybe<TTo>> Lift<TFrom, TTo>(Func<TFrom, TTo> f)
            => from => 
                from is Just<TFrom> just
                ? f(just.Value).ReturnMaybe()
                : new Nothing<TTo>();
    }
}