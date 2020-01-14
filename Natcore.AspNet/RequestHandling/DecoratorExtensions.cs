using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class DecoratorExtensions
	{
		public static IServiceCollection Decorate(this IServiceCollection services, Type serviceType, Type decoratorType)
		{
			List<(Type serviceType, Type decoratorType)> list = new List<(Type serviceType, Type decoratorType)>();

			if (serviceType.IsGenericTypeDefinition && decoratorType.IsGenericTypeDefinition)
			{
				foreach (var arguments in services
					.Where(x => !x.ServiceType.IsGenericTypeDefinition && x.ServiceType.IsGenericType && x.ServiceType.GetGenericTypeDefinition() == serviceType)
					.Select(x => x.ServiceType.GetGenericArguments())
				)
					list.Add((serviceType.MakeGenericType(arguments), decoratorType.MakeGenericType(arguments)));
			}
			else
				list.Add((serviceType, decoratorType));

			for(int i = 0; i < list.Count; i++)
			{
				var (st, dt) = list[i];
				ApplyDecorator(services, st, dt);
			}

			return services;
		}

		private static void ApplyDecorator(IServiceCollection services, Type serviceType, Type decoratorType)
		{
			var descriptors = services.Where(x => x.ServiceType == serviceType).ToArray();

			for(int i = 0; i < descriptors.Length; i++)
			{
				var descriptor = descriptors[i];
				int index = services.IndexOf(descriptor);

				services[index] = CreateDecoratedDescriptor(decoratorType, descriptor);
			}
		}

		private static ServiceDescriptor CreateDecoratedDescriptor(Type decoratorType, ServiceDescriptor baseDescriptor)
		{
			return new ServiceDescriptor(
				baseDescriptor.ServiceType,
				p =>
				{
					object baseInstance = null;
					if (baseDescriptor.ImplementationInstance != null)
						baseInstance = baseDescriptor.ImplementationInstance;
					else if (baseDescriptor.ImplementationType != null)
						baseInstance = ActivatorUtilities.GetServiceOrCreateInstance(p, baseDescriptor.ImplementationType);
					else
						baseInstance = baseDescriptor.ImplementationFactory(p);

					return ActivatorUtilities.CreateInstance(p, decoratorType, baseInstance);
				},
				baseDescriptor.Lifetime
			);
		}
	}
}
