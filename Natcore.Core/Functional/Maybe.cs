using System;

namespace Natcore.Core.Functional
{
	public struct Maybe<T>
	{
		public static Maybe<T> Some(T value) => new(value, hasValue: true);
		public static Maybe<T> None => new(value: default, hasValue: false);

		private readonly bool _hasValue;
		private readonly T _value;
		private Maybe(T value, bool hasValue)
		{
			_hasValue = hasValue;
			_value = value;
		}

		public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
			=> _hasValue ? some(_value) : none();

		public Maybe<TResult> Map<TResult>(Func<T, TResult> selector)
			=> _hasValue ? Maybe<TResult>.Some(selector(_value)) : Maybe<TResult>.None;

		public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> selector)
			=> _hasValue ? selector(_value) : Maybe<TResult>.None;

		public override string ToString()
			=> _hasValue ? $"Some {_value}" : "None";
	}

	public static class MaybeLinqExtensions
	{
		public static Maybe<TResult> Select<T, TResult>(this Maybe<T> source, Func<T, TResult> selector)
			=> source.Map(selector);

		public static Maybe<TResult> SelectMany<T, TResult>(this Maybe<T> source, Func<T, Maybe<TResult>> selector)
			=> SelectMany(source, selector, (_, r) => r);

		public static Maybe<TResult> SelectMany<T, TSelect, TResult>(this Maybe<T> source, Func<T, Maybe<TSelect>> selector, Func<T, TSelect, TResult> resultSelector)
			=> source.Bind(t => selector(t).Map(r => resultSelector(t, r)));
	}
}
