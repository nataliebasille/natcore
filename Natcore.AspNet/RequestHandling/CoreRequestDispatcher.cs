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

        public Task<IActionResult> DispatchAsync<TParams>(TParams parameters)
        {
			IRequestHandler<TParams> handler = _serviceProvider.GetRequiredService<IRequestHandler<TParams>>();

            return handler.HandleAsync(parameters);
        }
    }
}
