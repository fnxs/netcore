using System;
using System.Threading.Tasks;

namespace FunctionalExtensions.MaybeMonad
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
        /// <summary>
        /// ReturnMaybe :: a -> Maybe a
        /// </summary>
        public static Maybe<T> ReturnMaybe<T>(this T value) 
            => new Just<T>(value);
        
        /// <summary>
        /// ReturnMaybe :: Maybe a -> Maybe a
        /// </summary>
        public static Maybe<T> ReturnMaybe<T>(this Maybe<T> value)
            => value;
        
        /// <summary>
        /// ReturnMaybe :: Nothing a -> Maybe a
        /// </summary>
        public static Maybe<T> ReturnMaybe<T>(this Nothing<T> nothing)
            => nothing;
        
        /// <summary>
        /// ReturnMaybe :: Just a -> Maybe a
        /// </summary>
        public static Maybe<T> ReturnMaybe<T>(this Just<T> just)
            => just;
        
        /// <summary>
        /// Map :: Maybe a -> (a -> b) -> Maybe b
        /// </summary>
        public static Maybe<TTo> Map<TFrom, TTo>(this Maybe<TFrom> from, Func<TFrom, TTo> f)
            => from is Just<TFrom> just
                ? f(just.Value).ReturnMaybe()
                : new Nothing<TTo>();

        /// <summary>
        /// Bind :: Maybe a -> (a -> Maybe b) -> Maybe b 
        /// </summary>
        public static Maybe<TTo> Bind<TFrom, TTo>(this Maybe<TFrom> from, Func<TFrom, Maybe<TTo>> f)
            => from is Just<TFrom> just
                ? f(just.Value)
                : new Nothing<TTo>();

        /// <summary>
        /// BindAsync :: Task Maybe a -> (a -> Task Maybe b) -> Task Maybe b
        /// </summary>
        public static async Task<Maybe<TTo>> BindAsync<TFrom, TTo>(this Task<Maybe<TFrom>> taskFrom, Func<TFrom, Task<Maybe<TTo>>> f)
        {
            var from = await taskFrom;
            return from is Just<TFrom> just
                ? await f(just.Value)
                : new Nothing<TTo>();
        }

        /// <summary>
        /// Compose :: Maybe a -> Maybe b -> Maybe b
        /// </summary>
        public static Maybe<TTo> Compose<TFrom, TTo>(this Maybe<TFrom> from, Maybe<TTo> to)
            => from.Bind(ignored => to);
        
        /// <summary>
        /// Lift :: (a -> b) -> (Maybe a -> Maybe b)
        /// </summary>
        public static Func<Maybe<TFrom>, Maybe<TTo>> Lift<TFrom, TTo>(Func<TFrom, TTo> f)
            => from => 
                from is Just<TFrom> just
                ? f(just.Value).ReturnMaybe()
                : new Nothing<TTo>();
    }
}