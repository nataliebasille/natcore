using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Natcore.AspNet.ExceptionHandling
{
	public static class ExceptionLoggingServiceProviders
	{
		public static IServiceCollection AddExceptionLogging(this IServiceCollection services)
		{
			services.TryAddTransient<IErrorIdGenerator, DefaultErrorIdGenerator>();
			return services;
		}
	}
}
