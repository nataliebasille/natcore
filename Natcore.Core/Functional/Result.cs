using System;

namespace Natcore.Core.Functional
{
	public class Result<TSuccess, TFailure>
	{
		public static Result<TSuccess, TFailure> Success(TSuccess success)
			=> new Result<TSuccess, TFailure> { _successValue = success, _isSuccess = true };

		public static Result<TSuccess, TFailure> Failure(TFailure failure)
			=> new Result<TSuccess, TFailure> { _failureValue = failure, _isSuccess = false };

		private TSuccess _successValue;
		private TFailure _failureValue;

		private bool _isSuccess;

		public TResult Match<TResult>(
			Func<TSuccess, TResult> success,
			Func<TFailure, TResult> failure
		) => _isSuccess ? success(_successValue) : failure(_failureValue);

		public Result<TNext, TFailure> Map<TNext>(Func<TSuccess, TNext> selector)
			=> _isSuccess
			? Result<TNext, TFailure>.Success(selector(_successValue))
			: Result<TNext, TFailure>.Failure(_failureValue);

		public Result<TNext, TFailure> Bind<TNext>(Func<TSuccess, Result<TNext, TFailure>> selector)
			=> _isSuccess
			? selector(_successValue)
			: Result<TNext, TFailure>.Failure(_failureValue);

		public override string ToString()
			=> _isSuccess
			? $"Success ({_successValue})"
			: $"Failure ({_failureValue})";
	}

	public static class ResultExtensions
	{
		public static Result<TNext, TFailure> Select<TSuccess, TFailure, TNext>(this Result<TSuccess, TFailure> source, Func<TSuccess, TNext> selector)
			=> source.Map(selector);

		public static Result<TNext, TFailure> SelectMany<TSuccess, TFailure, TNext>(this Result<TSuccess, TFailure> source, Func<TSuccess, Result<TNext, TFailure>> selector)
			=> SelectMany(source, selector, (_, n) => n);

		public static Result<TResult, TFailure> SelectMany<TSuccess, TFailure, TNext, TResult>(this Result<TSuccess, TFailure> source, Func<TSuccess, Result<TNext, TFailure>> selector, Func<TSuccess, TNext, TResult> resultSelector)
			=> source.Bind(s => selector(s).Map(n => resultSelector(s, n)));
	}
}
