using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Natcore.EntityFramework.Logging
{
	public static class DatabaseLoggerFactoryExtensions
	{
		public static ILoggerFactory AddDatabaseLogger<TContext>(this ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
			where TContext : DbContext
		{
			loggerFactory.AddProvider(new DatabaseLoggerProvider<TContext>(serviceProvider));
			return loggerFactory;
		}
	}
}
