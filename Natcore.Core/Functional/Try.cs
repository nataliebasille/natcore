using System;

namespace Natcore.Core.Functional
{
	public class Try<T>
	{
		private readonly Result<T, Exception> _result;

		internal Try(Result<T, Exception> result)
		{
			_result = result;
		}

		public TResult Match<TResult>(Func<T, TResult> success, Func<Exception, TResult> failure)
			=> _result.Match(success, failure);

		public Try<TResult> Map<TResult>(Func<T, TResult> action)
			=> Bind(s => Try.Action(() => action(s)));

		public Try<TResult> Bind<TResult>(Func<T, Try<TResult>> selector)
			=> _result.Match(
				success: s => Try
					.Action(() => selector(s)).Match(
						success: t => t,
						failure: e => Try.AsFailure<TResult>(e)
					),
				failure: f => Try.AsFailure<TResult>(f)
			);

		public override string ToString() => _result.ToString();
	}

	public static class Try
	{
		public static Try<T> Action<T>(Func<T> action)
		{
			try
			{
				return AsSuccess(action());
			}
			catch (Exception ex)
			{
				return AsFailure<T>(ex);
			}
		}

		public static Try<T> AsSuccess<T>(T result) => new Try<T>(Result<T, Exception>.Success(result));
		public static Try<T> AsFailure<T>(Exception ex) => new Try<T>(Result<T, Exception>.Failure(ex));
	}

	public static class TryLinqExtensions
	{
		public static Try<TResult> Select<T, TResult>(this Try<T> source, Func<T, TResult> selector)
			=> source == null
			? Try.AsFailure<TResult>(new ArgumentNullException(nameof(source)))
			: selector == null
			? Try.AsFailure<TResult>(new ArgumentNullException(nameof(selector)))
			: source.Map(selector);

		public static Try<TResult> SelectMany<T, TResult>(this Try<T> source, Func<T, Try<TResult>> selector)
			=> SelectMany(source, selector, (_, n) => n);

		public static Try<TResult> SelectMany<T, TNext, TResult>(this Try<T> source, Func<T, Try<TNext>> selector, Func<T, TNext, TResult> resultSelector)
			=> source == null
			? Try.AsFailure<TResult>(new ArgumentNullException(nameof(source)))
			: selector == null
			? Try.AsFailure<TResult>(new ArgumentNullException(nameof(selector)))
			: resultSelector == null
			? Try.AsFailure<TResult>(new ArgumentNullException(nameof(resultSelector)))
			: source.Bind(t => selector(t).Map(n => resultSelector(t, n)));
	}
}
