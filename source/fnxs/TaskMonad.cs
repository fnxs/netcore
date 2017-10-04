using System;
using System.Threading.Tasks;

namespace FunctionalExtensions.TaskMonad
{
    public static class TaskExtensions
    {
        /// <summary>
        /// ReturnTask :: a -> Task a
        /// </summary>
        public static Task<T> ReturnTask<T>(this T value)
            => Task.FromResult(value);
        
        /// <summary> 
        /// ReturnTask :: Task a -> Task a
        /// </summary>
        public static Task<T> ReturnTask<T>(this Task<T> value)
            => value;

        /// <summary>
        /// Map :: Task a -> (a -> b) -> Task b 
        /// </summary>
        public static async Task<TTo> Map<TFrom, TTo>(this Task<TFrom> from, Func<TFrom, TTo> f)
            => f(await from);
        
        /// <summary>
        /// Bind :: Task a -> (a -> Task b) -> Task b
        /// </summary>
        public static async Task<TTo> Bind<TFrom, TTo>(this Task<TFrom> from, Func<TFrom, Task<TTo>> f)
            => await f(await from);

        /// <summary>
        /// Compose :: Task a -> Task b -> Task b
        /// </summary>
        public static Task<TTo> Compose<TTo, TFrom>(this Task<TFrom> from, Task<TTo> to)
            => from.Bind(ignored => to);
        
        /// <summary>
        /// Lift :: (a -> b) -> (Task a -> Task b)
        /// </summary>
        public static Func<Task<TFrom>, Task<TTo>> Lift<TFrom, TTo>(Func<TFrom, TTo> f)
            => async from 
                => f(await from);
    }
}