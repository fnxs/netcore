using System;

namespace FunctionalExtensions.Maybe
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

        public TL Left
        { 
            get => IsLeft
                    ? _left
                    : throw new ApplicationException("left");
        }
        public TR Right
        { 
            get => !IsLeft
                    ? _right
                    : throw new ApplicationException("right");
        }

    }

    public static class EitherExtensions
    {
        /// <summary>
        /// ReturnEither :: b -> M a b
        /// </summary>
        public static Either<TL, TR> ReturnEither<TL, TR>(this TR right)
            => ReturnEither<TL, TR>(right);
        
        /// <summary>
        /// ReturnEitherLeft :: a -> M a b
        /// </summary>
        public static Either<TL, TR> ReturnEitherLeft<TL, TR>(this TL left)
            => new Either<TL, TR>(left);
        
        /// <summary>
        /// ReturnEitherRight :: b -> M a b
        /// </summary>
        public static Either<TL, TR> ReturnEitherRight<TL, TR>(this TR right)
            => new Either<TL, TR>(right);
        
        /// <summary>
        /// Bind :: M a b -> (b -> M a c) -> M a c
        /// </summary>
        public static Either<TL, TRTo> Bind<TL, TRFrom, TRTo>(this Either<TL, TRFrom> either, Func<TRFrom, Either<TL, TRTo>> f)
            => either.IsLeft
                ? new Either<TL, TRTo>(either.Left)
                : f(either.Right);

        /// <summary>
        /// Bind :: M a b -> M a c -> M a c
        /// </summary>
        public static Either<TLeft, TToRight> Compose<TLeft, TFromRight, TToRight>(this Either<TLeft, TFromRight> from, Either<TLeft, TToRight> to)
            => from.Bind(ignored => to);

        /// <summary>
        /// Lift :: (a -> b) -> (M a b -> M a c)
        /// </summary>
        public static Func<Either<TLeft, TFromRight>, Either<TLeft, TToRight>> Lift<TLeft, TFromRight, TToRight>(Func<TFromRight, TToRight> f)
            => from
                => from.IsLeft
                    ? new Either<TLeft, TToRight>(from.Left)
                    : f(from.Right).ReturnEitherRight<TLeft, TToRight>();
    }
}