using System;

namespace FunctionalExtensions.Maybe
{
    public class Either<TLeft, TRight> 
    {
        private readonly TLeft _left;
        private readonly TRight _right;
        internal bool IsLeft { get; }

        internal Either(TLeft left)
        {
            _left = left;
            _right = default(TRight);
            IsLeft = true;
        }

        internal Either(TRight right)
        {
            _right = right;
            _left = default(TLeft);
            IsLeft = false;
        }

        public TLeft Left
        { 
            get
            {
                if (IsLeft)
                    return _left;
                throw new ApplicationException("left");
            } 
        }
        public TRight Right
        { 
            get
            {
                if (!IsLeft)
                    return _right;
                throw new ApplicationException("right");
            } 
        }

    }

    public static class EitherExtensions
    {
        public static Either<TLeft, TRight> ReturnEither<TLeft, TRight>(this TRight right)
            => ReturnEither<TLeft, TRight>(right);
        
        public static Either<TLeft, TRight> ReturnEitherLeft<TLeft, TRight>(this TLeft left)
            => new Either<TLeft, TRight>(left);
        
        public static Either<TLeft, TRight> ReturnEitherRight<TLeft, TRight>(this TRight right)
            => new Either<TLeft, TRight>(right);
        
        public static Either<TLeft, TToRight> Bind<TLeft, TFromRight, TToRight>(this Either<TLeft, TFromRight> either, Func<TFromRight, Either<TLeft, TToRight>> f)
            => either.IsLeft
                ? new Either<TLeft, TToRight>(either.Left)
                : f(either.Right);
        
        public static Either<TLeft, TToRight> Compose<TLeft, TFromRight, TToRight>(this Either<TLeft, TFromRight> from, Either<TLeft, TToRight> to)
            => from.Bind(ignored => to);
    }
}