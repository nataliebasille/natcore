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
                .Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IRequestHandler<>))
            )
            .Select(x => new
            {
                ParamsType = x.GetInterfaces().First(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IRequestHandler<>)).GetGenericArguments()[0],
                ImplementationType = x
            });

            foreach (var handler in handlerTypes)
            {
                Type serviceType = typeof(IRequestHandler<>).MakeGenericType(handler.ParamsType);
                Type validatorHandlerType = typeof(ModelValidationRequestHandler<>).MakeGenericType(handler.ParamsType);
				
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
