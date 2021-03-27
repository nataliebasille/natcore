using Microsoft.AspNetCore.Http;
using System;

namespace Natcore.AspNet.ProblemDetails
{
	public class ProblemDetailsOptions
	{
		public Func<HttpContext, bool> IsProblem { get; set; } = context => context.Response.StatusCode >= 400 && context.Response.StatusCode < 600;
	}
}
