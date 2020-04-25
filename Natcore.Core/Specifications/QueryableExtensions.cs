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
    }
}
