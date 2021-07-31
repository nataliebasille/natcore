using Natcore.Core.Exceptions;
using Natcore.Core.Specifications;

namespace System.Linq
{
    public static class QueryableExtensions
    {
        public static IQueryable<TResult> Specify<TQuery, TResult>(this IQueryable<TQuery> query, ISpecification<TQuery, TResult> specification)
        {
            ThrowIf.Argument.IsNull(query, nameof(query));
            ThrowIf.Argument.IsNull(specification, nameof(specification));

            return specification.Build(query);
        }

        public static ISpecification<TQuery> And<TQuery>(this ISpecification<TQuery> source, ISpecification<TQuery> andSpec)
            => new ChainSpecification<TQuery>(source, andSpec);

        public static ISpecification<TQuery, TResult> And<TQuery, TResult>(this ISpecification<TQuery> source, ISpecification<TQuery, TResult> andSpec)
            => new ChainSpecification<TQuery, TQuery, TResult>(source, andSpec);

        public static ISpecification<TQuery, TResult> And<TQuery, TResult>(this ISpecification<TQuery, TResult> source, ISpecification<TResult> andSpec)
            => new ChainSpecification<TQuery, TResult, TResult>(source, andSpec);

        public static ISpecification<TQuery, TNext> And<TQuery, TResult, TNext>(this ISpecification<TQuery, TResult> source, ISpecification<TResult, TNext> andSpec)
            => new ChainSpecification<TQuery, TResult, TNext>(source, andSpec);

        private class ChainSpecification<TQuery> : ChainSpecification<TQuery, TQuery, TQuery>, ISpecification<TQuery>
        {
            public ChainSpecification(ISpecification<TQuery> left, ISpecification<TQuery> right)
                : base(left, right)
            { }
        }

        private class ChainSpecification<TQuery, TFirst, TSecond> : ISpecification<TQuery, TSecond>
        {
            private readonly ISpecification<TQuery, TFirst> _left;
            private readonly ISpecification<TFirst, TSecond> _right;

            public ChainSpecification(ISpecification<TQuery, TFirst> left, ISpecification<TFirst, TSecond> right)
            {
                _left = left;
                _right = right;
            }

            public IQueryable<TSecond> Build(IQueryable<TQuery> query)
                => _right.Build(_left.Build(query));
        }
    }
}
