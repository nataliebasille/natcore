using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Natcore.AspNet
{
	public static class RequestResult
	{
		public static IActionResult Forbidden() => new ForbidResult();
		public static IActionResult NoContent() => new NoContentResult();
		public static IActionResult NotFound() => new NotFoundResult();
		public static IActionResult BadRequest(object error) => new BadRequestObjectResult(error);
		public static IActionResult BadRequest(ModelStateDictionary modelState) => new BadRequestObjectResult(modelState);
		public static IActionResult Ok() => new OkResult();
		public static IActionResult Ok(object payload) => new OkObjectResult(payload);
	}
}
