using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace Natcore.AspNet.Testing.ActionResultAssertions
{
	public static class AssertionExtensions
	{
		public static ResultAssertion IsOK(this ActionResult result)
			=> IsOK(new ResultAssertion(result));

		public static ResultAssertion IsOK<T>(this ActionResult<T> result)
			=> IsOK(new ResultAssertion(result.Result));

		public static ResultAssertion IsOK(this ResultAssertion assertion)
			=> AssertStatusCondition(assertion, HttpStatusCode.OK);

		public static ResultAssertion IsNoContent(this ActionResult result)
			=> IsNoContent(new ResultAssertion(result));

		public static ResultAssertion IsNoContent<T>(this ActionResult<T> result)
			=> IsNoContent(new ResultAssertion(result.Result));

		public static ResultAssertion IsNoContent(this ResultAssertion assertion)
			=> AssertStatusCondition(assertion, HttpStatusCode.NoContent);

		public static ResultAssertion IsForbidden(this ActionResult result)
			=> IsForbidden(new ResultAssertion(result));

		public static ResultAssertion IsForbidden<T>(this ActionResult<T> result)
			=> IsForbidden(new ResultAssertion(result.Result));

		public static ResultAssertion IsForbidden(this ResultAssertion assertion)
			=> AssertStatusCondition(assertion, HttpStatusCode.Forbidden);

		public static ResultAssertion IsBadRequest(this ActionResult result)
			=> IsBadRequest(new ResultAssertion(result));

		public static ResultAssertion IsBadRequest<T>(this ActionResult<T> result)
			=> IsBadRequest(new ResultAssertion(result.Result));

		public static ResultAssertion IsBadRequest(this ResultAssertion assertion)
			=> AssertStatusCondition(assertion, HttpStatusCode.BadRequest);

		public static ResultAssertion HasBody(this ActionResult result)
			=> HasBody(new ResultAssertion(result));

		public static ResultAssertion HasBody(this ActionResult result, object body)
			=> HasBody(new ResultAssertion(result), body);

		public static ResultAssertion HasBody<T>(this ActionResult<T> result)
			=> HasBody(new ResultAssertion(result.Result));

		public static ResultAssertion HasBody<T>(this ActionResult<T> result, T body)
			=> HasBody(new ResultAssertion(result.Result), body);

		public static ResultAssertion HasBody(this ResultAssertion assertion)
		{
			Execute.Assertion
				.ForCondition(assertion.Subject is ObjectResult)
				.FailWith("Expected result to have body, but none was found");

			return assertion;
		}

		public static ResultAssertion HasBody(this ResultAssertion assertion, object body)
		{
			assertion.HasBody();

			(assertion.Subject as ObjectResult)?.Value
				.Should()
				.BeEquivalentTo(body);

			return assertion;
		}

		private static IDictionary<Type, int> actionResultToStatusMapping =
			new Dictionary<Type, int>()
			{
				[typeof(ForbidResult)] = (int)HttpStatusCode.Forbidden
			};

		private static ResultAssertion AssertStatusCondition(ResultAssertion assertion, HttpStatusCode statusCode)
		{
			IActionResult result = assertion.Subject;
			int actual =
				(result as StatusCodeResult)?.StatusCode ??
				(result as ObjectResult)?.StatusCode ??
				(actionResultToStatusMapping.TryGetValue(result.GetType(), out int s) ? s : 0);

			Execute.Assertion
				.ForCondition(actual != 0)
				.FailWith("Expected result to have status code {0}, but the status code could not be determined", (int)statusCode);

			Execute.Assertion
				.ForCondition(actual == (int)statusCode)
				.FailWith("Expected result to have status code {0}, but found {1}", (int)statusCode, actual);

			return assertion;
		}
	}
}
