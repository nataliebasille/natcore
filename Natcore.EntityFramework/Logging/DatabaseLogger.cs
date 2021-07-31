using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Natcore.Core.Clock;
using System;
using System.Text.Json;

namespace Natcore.EntityFramework.Logging
{
	public class DatabaseLogger<TContext> : ILogger
		where TContext: DbContext
	{
		private readonly IServiceProvider _serviceProvider;
		public DatabaseLogger(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return new IgnoredDisposable();
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return true;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (ShouldLog(logLevel, eventId, state))
			{
				IClock clock = _serviceProvider.GetService<IClock>() ?? new InternalClock();
				string message;
				if (formatter != null)
					message = formatter(state, exception);
				else if (state != null && state.GetType() != typeof(string))
					message = JsonSerializer.Serialize(state, new JsonSerializerOptions
					{
						WriteIndented = true
					});
				else
					message = state as string;

				if (string.IsNullOrEmpty(message))
					message = "<Message not provided>";

				var log = new EventLog
				{
					Message = message,
					Level = logLevel.ToString(),
					EventID = eventId == 0 ? null : (int?)eventId.Id,
					Exception = exception?.ToString(),
					Timestamp = clock.CurrentUtcTime()
				};

				using (var scope = _serviceProvider.CreateScope())
				{
					HttpContext httpContext = scope.ServiceProvider.GetService<IHttpContextAccessor>()?.HttpContext;

					if (httpContext != null)
					{
						log.Browser = httpContext.Request.Headers["User-Agent"];
						log.Username = httpContext.User.Identity.Name;
						log.Url = httpContext.Request.Path;

						try
						{
							log.Host = httpContext.Connection.LocalIpAddress?.ToString();
						}
						catch (ObjectDisposedException)
						{
							log.Host = "<httpcontext has been disposed>";
						}
					}

					var context = scope.ServiceProvider.GetRequiredService<TContext>();
					var storage = new EfCoreStorage(context);
					storage.Add(log);

					storage.SaveChanges();
				}
			}
		}

		private bool ShouldLog(LogLevel logLevel, EventId eventId, object state)
		{
			if (((eventId.Name?.Contains("Microsoft.EntityFrameworkCore") ?? false) && !(state?.ToString()?.Contains("SELECT") ?? false))
				|| (state?.ToString()?.Contains("INSERT INTO \"EventLog\"") ?? false)
				
			)
				return false;

			return IsEnabled(logLevel);
		}

		private class IgnoredDisposable : IDisposable
		{
			public void Dispose() { }
		}
	}
}
