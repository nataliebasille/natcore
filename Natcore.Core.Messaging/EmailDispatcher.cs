using Microsoft.Extensions.DependencyInjection;
using Natcore.Core.Exceptions;
using System;
using System.Threading.Tasks;

namespace Natcore.Core.Messaging
{
    public interface IEmailDispatcher
    {
        Task DispatchAsync<TOptions>(TOptions options);
    }

    public class EmailDispatcher : IEmailDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailClient _client;

        public EmailDispatcher(IServiceProvider serviceProvider, IEmailClient client)
        {
            _serviceProvider = serviceProvider;
            _client = client;
        }

        public async Task DispatchAsync<TOptions>(TOptions options)
        {
            ThrowIf.Argument.IsNull(options, nameof(options));

            Type serviceType = typeof(IEmailMessageBuilder<>).MakeGenericType(options.GetType());

            dynamic builder = _serviceProvider.GetRequiredService(serviceType);

            EmailMessage emailMessage = builder.Build((dynamic)options);

            await _client.SendAsync(emailMessage);
        }
    }
}
