using System;
using System.Threading.Tasks;

namespace FunctionalExtensions.Task
{
    public static class TaskExtensions
    {
        public static Task<T> ReturnTask<T>(this T value)
            => Task<T>.FromResult(value);
        
        public static Task<T> ReturnTask<T>(this Task<T> value)
            => value;
        
        public static async Task<TTo> Bind<TFrom, TTo>(this Task<TFrom> from, Func<TFrom, Task<TTo>> f)
        {
            var result = await from;
            return await f(result);
        }

        public static Task<TTo> Compose<TTo, TFrom>(this Task<TFrom> from, Task<TTo> to)
            => from.Bind(ignored => to);
    }
}