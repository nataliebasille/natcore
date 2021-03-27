using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;
using Natcore.AspNet.ProblemDetails;

namespace Natcore.AspNet
{
	public static class RequestResult
	{
		public static IActionResult Forbidden(string reason = null) => HandleProblem(new StatusCodeProblemDetail(403, reason));
		public static IActionResult NoContent() => new NoContentResult();
		public static IActionResult NotFound(string reason = null) => HandleProblem(new StatusCodeProblemDetail(404, reason));
		public static IActionResult BadRequest(object error) => HandleProblem(new BadRequestProblemDetail(error));
		public static IActionResult BadRequest(ModelStateDictionary modelState) => HandleProblem(new BadRequestProblemDetail(modelState));
		public static IActionResult Ok() => new OkResult();
		public static IActionResult Ok(object payload) => new OkObjectResult(payload);

		private static IActionResult HandleProblem(ProblemDetail problem)
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
