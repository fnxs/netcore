using System;
using System.Threading.Tasks;

namespace FunctionalExtensions.EitherMonad
{
    public class Either<TL, TR>
    {
        private readonly TL _left;
        private readonly TR _right;
        internal bool IsLeft { get; }

        internal Either(TL left)
        {
            _left = left;
            _right = default(TR);
            IsLeft = true;
        }

        internal Either(TR right)
        {
            _right = right;
            _left = default(TL);
            IsLeft = false;
        }

        public TL Left => IsLeft
            ? _left
            : throw new ApplicationException("left");

        public TR Right => !IsLeft
            ? _right
            : throw new ApplicationException("right");
    }

    public static class EitherExtensions
    {
        /// <summary>
        /// ReturnEither :: b -> Either a b
        /// </summary>
        public static Either<TL, TR> ReturnEither<TL, TR>(this TR right)
            => ReturnEitherRight<TL, TR>(right);

        /// <summary>
        /// ReturnEitherLeft :: a -> Either a b
        /// </summary>
        public static Either<TL, TR> ReturnEitherLeft<TL, TR>(this TL left)
            => new Either<TL, TR>(left);

        /// <summary>
        /// ReturnEitherRight :: b -> Either a b
        /// </summary>
        public static Either<TL, TR> ReturnEitherRight<TL, TR>(this TR right)
            => new Either<TL, TR>(right);

        /// <summary>
        /// Map :: Either a b -> (b -> c) -> Either a c
        /// </summary>
        public static Either<TL, TRTo> Map<TL, TRFrom, TRTo>(this Either<TL, TRFrom> from, Func<TRFrom, TRTo> f)
            => from.IsLeft
                ? from.Left.ReturnEitherLeft<TL, TRTo>()
                : f(from.Right).ReturnEither<TL, TRTo>();
        
        /// <summary>
        /// Bind :: Either a b -> (b -> Either a c) -> Either a c
        /// </summary>
        public static Either<TL, TRTo> Bind<TL, TRFrom, TRTo>(
            this Either<TL, TRFrom> either,
            Func<TRFrom, Either<TL, TRTo>> f)
            => either.IsLeft
                ? either.Left.ReturnEitherLeft<TL, TRTo>()
                : f(either.Right);

        /// <summary>
        /// BindAsync :: Task Either a b -> (b -> Task Either a c) -> Task Either a c
        /// </summary>
        public static async Task<Either<TL, TRTo>> BindAsync<TL, TRFrom, TRTo>(
            this Task<Either<TL, TRFrom>> taskEither,
            Func<TRFrom, Task<Either<TL, TRTo>>> f)
        {
            var either = await taskEither;
            return either.IsLeft
                ? new Either<TL, TRTo>(either.Left)
                : await f(either.Right);
        }

        /// <summary>
        /// Compose :: Either a b -> Either a c -> Either a c
        /// </summary>
        public static Either<TL, TR> Compose<TL, TFromRight, TR>(
            this Either<TL, TFromRight> from,
            Either<TL, TR> to)
            => from.Bind(ignored => to);

        /// <summary>
        /// Lift :: (a -> b) -> (Either a b -> Either a c)
        /// </summary>
        public static Func<Either<TL, TRFrom>, Either<TL, TRTo>> Lift<TL, TRFrom, TRTo>(
            Func<TRFrom, TRTo> f)
            => from
                => from.IsLeft
                    ? from.Left.ReturnEitherLeft<TL, TRTo>()
                    : f(from.Right).ReturnEitherRight<TL, TRTo>();
    }
}