using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natcore.AspNet.ProblemDetails
{
	public static class ProblemDetailExtensions
	{
		public static IServiceCollection AddProblemDetails(this IServiceCollection services)
		{
			services.TryAddTransient<IProblemDetailFactory, ProblemDetailFactory>();
			services.TryAddTransient<IDetailIdGenerator, DefaultDetailIdGenerator>();
			services.AddSingleton(new ProblemDetailsOptions());

			return services;
		}

		public static IApplicationBuilder UseProblemDetails(this IApplicationBuilder app)
			=> app.UseMiddleware<ProblemDetailsMiddleware>();
	}
}
