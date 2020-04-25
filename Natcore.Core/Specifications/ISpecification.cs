using System;
using System.Collections.Generic;
using System.Linq;

namespace Natcore.Core.Specifications
{
    public interface ISpecification<in TIn, out TOut>
    {
        IQueryable<TOut> Build(IQueryable<TIn> query);
    }

    public interface ISpecification<T> : ISpecification<T, T> { }
}
