using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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

        private static IServiceCollection Add(IServiceCollection services)
            => services
            .AddTransient<IMailTransport, SmtpClient>(p => new SmtpClient())
            .AddTransient<IEmailClient, EmailClient>();
    }
}
