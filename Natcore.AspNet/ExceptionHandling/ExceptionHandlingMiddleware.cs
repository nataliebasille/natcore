using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;

namespace Natcore.AspNet.ExceptionHandling
{
	public static class ExceptionHandlingMiddleware
	{
		private static int id = 1000;

		public static IApplicationBuilder UseExceptionLogger(this IApplicationBuilder app, ILoggerFactory loggerFactory)
		{
			return app.UseExceptionHandler(onError =>
			{
				onError.Run(async context =>
				{
					var logger = loggerFactory.CreateLogger("Natcore.AspNet.ExceptionHandling");

					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					context.Response.ContentType = "application/json";

					var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
					if (contextFeature != null)
					{
						int errorID = id++;
						logger.LogError(errorID, contextFeature.Error, $"Unhandled exception.  Generated Event id is {errorID}");

						await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorModel()
						{
							Code = errorID.ToString(),
							Message = $"Oops, an unexpected error occurred{Environment.NewLine}Please contact the administrator"
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
