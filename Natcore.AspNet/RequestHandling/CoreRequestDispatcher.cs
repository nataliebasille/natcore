using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Natcore.AspNet
{
    public class CoreRequestDispatcher : IRequestDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        public CoreRequestDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<ActionResult> DispatchAsync(IRequest request)
        {
			Type serviceType = typeof(IRequestHandler<>).MakeGenericType(request.GetType());

			dynamic handler = _serviceProvider.GetRequiredService(serviceType);

            return handler.HandleAsync((dynamic)request);
        }

		public async Task<ActionResult> DispatchAsync<TResult>(IRequest<TResult> request)
		{
			Type serviceType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResult));

			dynamic handler = _serviceProvider.GetRequiredService(serviceType);

			var result = await handler.HandleAsync((dynamic)request);
			return result.Result;
		}
    }
}
