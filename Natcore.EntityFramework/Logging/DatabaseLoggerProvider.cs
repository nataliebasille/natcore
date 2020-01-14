using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Natcore.EntityFramework.Logging
{
	[ProviderAlias("Database")]
	public class DatabaseLoggerProvider<TContext> : ILoggerProvider
		where TContext : DbContext
	{
		private readonly IServiceProvider _serviceProvider;
		public DatabaseLoggerProvider(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public ILogger CreateLogger(string categoryName)
			=> new DatabaseLogger<TContext>(_serviceProvider);

		public void Dispose() { }
	}
}
