using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;
using Natcore.AspNet.ProblemDetails;

namespace Natcore.AspNet
{
	public static class RequestResult
	{
		public static ActionResult Forbidden(string reason = null) => HandleProblem(new StatusCodeProblemDetail(403, reason));
		public static ActionResult NoContent() => new NoContentResult();
		public static ActionResult NotFound(string reason = null) => HandleProblem(new StatusCodeProblemDetail(404, reason));
		public static ActionResult BadRequest(object error) => HandleProblem(new BadRequestProblemDetail(error));
		public static ActionResult BadRequest(ModelStateDictionary modelState) => HandleProblem(new BadRequestProblemDetail(modelState));
		public static ActionResult Ok() => new OkResult();
		public static ActionResult<T> Ok<T>(T payload) => new OkObjectResult(payload);

		private static ActionResult HandleProblem(ProblemDetail problem)
		{
			return new ObjectResult(problem)
			{
				StatusCode = problem.Status,
				ContentTypes = new MediaTypeCollection
				{
					new MediaTypeHeaderValue("application/problem+json")
				},
				DeclaredType = problem.GetType()
			};
		}
	}
}
