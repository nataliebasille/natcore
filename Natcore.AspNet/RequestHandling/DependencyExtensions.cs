using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace Natcore.AspNet
{
    public static class DependencyExtensions
    {
        public static IServiceCollection AddRequestHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            var handlerTypes = assemblies.SelectMany(x => x.GetTypes())
              .Where(x => x.IsClass && x.GetInterfaces()
                .Any(y => y.IsGenericType && (y.GetGenericTypeDefinition() == typeof(IRequestHandler<>) || y.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
            )
            .Select(x => new
            {
                RequestType = x.GetInterfaces().First(y => y.IsGenericType && (y.GetGenericTypeDefinition() == typeof(IRequestHandler<>) || y.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))).GetGenericArguments()[0],
				ResultType = x.GetInterfaces().FirstOrDefault(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))?.GetGenericArguments()[1],
				ImplementationType = x
            });

            foreach (var handler in handlerTypes)
            {
                Type serviceType = handler.ResultType != null
					? typeof(IRequestHandler<,>).MakeGenericType(handler.RequestType, handler.ResultType)
					: typeof(IRequestHandler<>).MakeGenericType(handler.RequestType);
                Type validatorHandlerType = handler.ResultType != null
					? typeof(ModelValidationRequestHandler<,>).MakeGenericType(handler.RequestType, handler.ResultType)
					: typeof(ModelValidationRequestHandler<>).MakeGenericType(handler.RequestType);
				
                services.TryAddTransient(handler.ImplementationType);
                services.TryAddTransient(serviceType, provider => Activator.CreateInstance(validatorHandlerType, new[] { provider.GetRequiredService(handler.ImplementationType), provider.GetService<ModelStateDictionary>() }));
            }

            services.TryAddTransient<IRequestDispatcher, CoreRequestDispatcher>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddTransient(p =>
            {
                var actionContentAccessor = p.GetRequiredService<IActionContextAccessor>();
                return actionContentAccessor.ActionContext?.ModelState;
            });

            return services;
        }
    }
}
