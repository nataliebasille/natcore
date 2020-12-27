using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Natcore.AspNet.ProblemDetails
{
	public class ProblemDetailsMiddleware
	{
		private static HashSet<string> _copyableHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            HeaderNames.AccessControlAllowCredentials,
            HeaderNames.AccessControlAllowHeaders,
            HeaderNames.AccessControlAllowMethods,
            HeaderNames.AccessControlAllowOrigin,
            HeaderNames.AccessControlExposeHeaders,
            HeaderNames.AccessControlMaxAge,
            HeaderNames.StrictTransportSecurity,
            HeaderNames.WWWAuthenticate,
        };

		private readonly RequestDelegate _next;
		private readonly ProblemDetailsOptions _options;
		private readonly ILogger<ProblemDetailsMiddleware> _logger;
		private readonly IActionResultExecutor<ObjectResult> _resultExecutor;

		public ProblemDetailsMiddleware(
			RequestDelegate next,
			IOptions<ProblemDetailsOptions> options,
			ILogger<ProblemDetailsMiddleware> logger,
			IActionResultExecutor<ObjectResult> resultExecutor
		)
		{
			_next = next;
			_options = options.Value;
			_logger = logger;
			_resultExecutor = resultExecutor;
		}

		public async Task Invoke(HttpContext context, IProblemDetailFactory factory)
		{
			Exception error = null;
			try
			{
				await _next(context);

				if (!_options.IsProblem(context))
					return;

			} catch(Exception ex)
			{
				error = ex;
			}

			if (context.Response.HasStarted)
			{
				if(error != null)
					ExceptionDispatchInfo.Capture(error).Throw();

				return;
			}

			var detail = factory.CreateDetail(context, error);

			ResetResponse(context, detail.Status);

			var actionContext = new ActionContext(context, context.GetRouteData() ?? new RouteData(), new ActionDescriptor());

			var result = new ObjectResult(detail)
			{
				StatusCode = detail.Status,
				ContentTypes = new MediaTypeCollection()
				{
					"application/problem+json"
				},
				DeclaredType = detail.GetType(),
			};

			await _resultExecutor.ExecuteAsync(actionContext, result);

			await context.Response.CompleteAsync();
		}

		private void ResetResponse(HttpContext context, int status)
		{
			var headers = new HeaderDictionary();

			// Preventing problem details from being cached
			headers.Append(HeaderNames.CacheControl, "no-cache, no-store, must-revalidate");
			headers.Append(HeaderNames.Pragma, "no-cache");
			headers.Append(HeaderNames.Expires, "0");

			foreach (var header in context.Response.Headers)
			{
				if (_copyableHeaders.Contains(header.Key))
					headers.Add(header);
			}

			context.Response.Clear();
			context.Response.StatusCode = status;

			foreach (var header in headers)
			{
				context.Response.Headers.Add(header);
			}
		}
	}
}
