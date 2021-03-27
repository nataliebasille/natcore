using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Natcore.AspNet.ProblemDetails;
using System;
using System.Text.Json;

namespace Natcore.AspNet.ExceptionHandling
{
	public static class ExceptionHandlingMiddleware
	{
		public static IApplicationBuilder UseExceptionLogger(this IApplicationBuilder app, ILoggerFactory loggerFactory)
		{
			return app.UseExceptionHandler(onError =>
			{
				onError.Run(async context =>
				{
					IErrorIdGenerator errorIdGenerator = app.ApplicationServices.GetService<IErrorIdGenerator>();

					if (errorIdGenerator == null)
						throw new InvalidOperationException($"To use exception logger, add a dependency for {typeof(IDetailIdGenerator)} or use AddExceptionLogger when registering your services");

					var logger = loggerFactory.CreateLogger("Natcore.AspNet.ExceptionHandling");

					var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
					if (contextFeature != null)
					{
						string referenceId = errorIdGenerator.NextID();
						logger.LogError(contextFeature.Error, $"Unhandled exception.  Reference ID: {referenceId}");

						await context.Response.WriteAsync(JsonSerializer.Serialize(new UnhandledExceptionProblemDetail(contextFeature.Error)
						{
							ReferenceID = referenceId,
							Title = "Unhandled Exception",
							Instance = context.Request.Path,
							Detail = $"Oops, an unexpected error occurred{Environment.NewLine}Please contact the administrator"
						}, new JsonSerializerOptions
						{
							PropertyNamingPolicy = JsonNamingPolicy.CamelCase
						}));
					}
				});
			});
		}
	}
}
