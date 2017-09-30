using System;

namespace FunctionalExtensions.Maybe
{
    public abstract class Maybe<T> 
    {
        public static Func<TFrom, Maybe<T>> Lift<TFrom>(Func<TFrom, T> f)
            => from
                => new Just<T>(f(from));
    }

    public class Nothing<T> : Maybe<T> { }

    public class Just<T> : Maybe<T>
    {
        public Just(T value) => Value = value;

        public T Value { get; }
    }

    public static class MaybeExtensions
    {
        // Return :: a -> Maybe a
        public static Maybe<T> Return<T>(this T value) 
            => new Just<T>(value);
        
        // Return :: M a -> M a
        public static Maybe<T> Return<T>(this Maybe<T> value)
            => value;
        
        // Return :: Nothing a -> M a
        public static Maybe<T> Return<T>(this Nothing<T> nothing)
            => nothing;
        
        // Return :: Just a -> M a
        public static Maybe<T> Return<T>(this Just<T> just)
            => just;

        // Bind :: M a -> (a -> M b) -> M b 
        public static Maybe<TTo> Bind<TFrom, TTo>(this Maybe<TFrom> from, Func<TFrom, Maybe<TTo>> f)
            => from is Just<TFrom> just
                ? f(just.Value)
                : new Nothing<TTo>();

        // Compose :: M a -> M b -> M b
        public static Maybe<TTo> Compose<TFrom, TTo>(this Maybe<TFrom> from, Maybe<TTo> to)
            => from is Just<TFrom>
                ? to
                : new Nothing<TTo>();
        
        // Lift :: M a -> (a -> b) -> M b
        public static Maybe<TTo> Lift<TFrom, TTo>(this TFrom from, Func<TFrom, TTo> f)
            => new Just<TTo>(f(from));            
    }
}