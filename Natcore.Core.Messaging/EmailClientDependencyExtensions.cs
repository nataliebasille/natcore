using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace Natcore.Core.Messaging
{
    public static class EmailClientDependencyExtensions
    {
        public static IServiceCollection AddSmtpEmailClient(this IServiceCollection services, Action<EmailClientOptions> optionsBuilder)
        {
            return Add(services.Configure(optionsBuilder));
        }

        public static IServiceCollection AddSmtpEmailClient(this IServiceCollection services, IConfiguration config)
        {
            return Add(services.Configure<EmailClientOptions>(config));
        }

        public static IServiceCollection AddEmailMessageBuilders(this IServiceCollection services, params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(x => x.GetTypes())
              .Where(x => x.IsClass && !x.IsAbstract && x.GetInterfaces()
                .Any(y => y.IsGenericType && (y.GetGenericTypeDefinition() == typeof(IEmailMessageBuilder<>)))
            );

            foreach(var implementationType in types)
            {
                var optionType = implementationType.GetInterfaces().First(y => y.IsGenericType && (y.GetGenericTypeDefinition() == typeof(IEmailMessageBuilder<>))).GetGenericArguments()[0];
                Type serviceType = typeof(IEmailMessageBuilder<>).MakeGenericType(optionType);

                services.TryAddTransient(implementationType);
                services.TryAddTransient(serviceType, implementationType);
            }

            services.TryAddTransient<IEmailDispatcher, EmailDispatcher>();

            return services;
        }

        private static IServiceCollection Add(IServiceCollection services)
            => services
            .AddTransient<IMailTransport, SmtpClient>(p => new SmtpClient())
            .AddTransient<IEmailClient, EmailClient>()
            .AddTransient<IEmailDispatcher, EmailDispatcher>();
    }
}
