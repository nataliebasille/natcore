using MailKit;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Natcore.Core.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Natcore.Core.Messaging
{
    public class EmailClient : IEmailClient
    {
        private readonly IMailTransport _transport;
        private readonly EmailClientOptions _options;
        public EmailClient(IMailTransport mailTransport, IOptions<EmailClientOptions> options)
        {
            _transport = mailTransport;
            _options = options.Value;
        }

        public Task SendAsync(EmailMessage message)
        {
            //changing
            var email = new MimeMessage();

            ThrowIf.Argument.IsNullOrEmpty(message.To, "message.To");
            ThrowIf.Argument.IsNull(message.From, "message.From");

            email.To.AddRange(message.To.Select(x => new MailboxAddress(x)));
            email.From.Add(new MailboxAddress(message.From));

            if (message.CC != null)
                email.Cc.AddRange(message.CC.Select(x => new MailboxAddress(x)));

            if (message.BCC != null)
                email.Bcc.AddRange(message.BCC.Select(x => new MailboxAddress(x)));

            email.Subject = message.Subject ?? string.Empty;

            var builder = GetBuilder(message.Format, message.Body);

            for(int i = 0; i < message.Resources.Count; i++)
            {
                builder.LinkedResources.Add(message.Resources[i].Url).ContentId = message.Resources[i].ID;
            }

            email.Body = builder.ToMessageBody();

            return PerformSend();

            async Task PerformSend()
            {
                await _transport.ConnectAsync(_options.Server, _options.Port, _options.UseSSL);

                if (_options.Authentication != null)
                    await _options.Authentication.AuthenticateAsync(_transport);

                await _transport.SendAsync(email);

                await _transport.DisconnectAsync(true);
            }
        }

        private BodyBuilder GetBuilder(EmailFormat format, string body)
        {
            var bodyBuilder = new BodyBuilder();

            if (format == EmailFormat.Text)
                bodyBuilder.TextBody = body;
            else if (format == EmailFormat.Html)
                bodyBuilder.HtmlBody = body;

            return bodyBuilder;
        }
    }
}
