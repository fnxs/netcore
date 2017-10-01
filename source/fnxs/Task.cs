using System;
using System.Threading.Tasks;

namespace FunctionalExtensions.Task
{
    public static class TaskExtensions
    {
        /// <summary>
        /// ReturnTask :: a -> M a
        /// </summary>
        public static Task<T> ReturnTask<T>(this T value)
            => Task<T>.FromResult(value);
        
        /// <summary> 
        /// ReturnTask :: M a -> M a
        /// </summary>
        public static Task<T> ReturnTask<T>(this Task<T> value)
            => value;
        
        /// <summary>
        /// Bind :: M a -> (a -> M b) -> M b 
        /// </summary>
        public static async Task<TTo> Bind<TFrom, TTo>(this Task<TFrom> from, Func<TFrom, Task<TTo>> f)
            => await f(await from);

        /// <summary>
        /// Compose :: M a -> M b -> M b
        /// </summary>
        public static Task<TTo> Compose<TTo, TFrom>(this Task<TFrom> from, Task<TTo> to)
            => from.Bind(ignored => to);
        
        /// <summary>
        /// Lift :: (a -> b) -> (M a -> M b)
        /// </summary>
        public static Func<Task<TFrom>, Task<TTo>> Lift<TFrom, TTo>(Func<TFrom, TTo> f)
            => async from 
                => f(await from);
    }
}