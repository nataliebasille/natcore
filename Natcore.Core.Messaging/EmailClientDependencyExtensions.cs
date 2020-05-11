using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Natcore.Core.Messaging
{
    public static class EmailClientDependencyExtensions
    {
        public static IServiceCollection AddSmtpEmailClient(this IServiceCollection services, Action<EmailClientOptions> optionsBuilder)
        {
            return services.AddTransient<IMailTransport, SmtpClient>(p => new SmtpClient())
                .Configure(optionsBuilder)
                .AddTransient<IEmailClient, EmailClient>();
        }
    }
}
