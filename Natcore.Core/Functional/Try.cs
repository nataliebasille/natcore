using System;
using System.Threading.Tasks;

namespace Natcore.Core.Functional
{
	public abstract record Try<T>
	{
		public Try<TResult> Map<TResult>(Func<T, TResult> action)
			=> Bind(s => Try.Action(() => action(s)));

		public Try<TResult> Bind<TResult>(Func<T, Try<TResult>> selector)
			=> this switch
			{
				TrySuccess<T> s => Try.Action(() => selector(s.Value)) switch
				{
					TrySuccess<Try<TResult>> innerSuccess => innerSuccess.Value,
					TryFailure<Try<TResult>> innerFailure => Try.AsFailure<TResult>(innerFailure.Error),
					_ => Try.AsFailure<TResult>(new Exception("Unhandled Try Match"))
				},
				TryFailure<T> f => Try.AsFailure<TResult>(f.Error),
				_ => Try.AsFailure<TResult>(new Exception("Unhandled Try Match"))
			};
	}

	public record TrySuccess<T>(T Value): Try<T>
	{
		public override string ToString() => $"[SUCCESS] ({Value})";
	}

	public record TryFailure<T>(Exception Error): Try<T>
	{
		public override string ToString() => $"[FAILURE] ({Error})";
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

		public static async Task<Try<T>> AsyncAction<T>(Func<Task<T>> action)
		{
			try
			{
				return AsSuccess(await action());
			}
			catch(Exception ex)
			{
				return AsFailure<T>(ex);
			}
		}

		public static Try<T> AsSuccess<T>(T result) => new TrySuccess<T>(result);
		public static Try<T> AsFailure<T>(Exception ex) => new TryFailure<T>(ex);
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
