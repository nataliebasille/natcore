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

        public Task Send(EmailMessage message)
        {
            var email = new MimeMessage();

            ThrowIf.Argument.IsNullOrEmpty(message.To, "message.To");
            ThrowIf.Argument.IsNull(message.From, "message.From");

            email.To.AddRange(message.To.Select(x => new MailboxAddress(x)));
            email.From.Add(new MailboxAddress(message.From));

            if (message.CC != null)
                email.Cc.AddRange(message.CC.Select(x => new MailboxAddress(x)));

            if (message.BCC != null)
                email.Bcc.AddRange(message.BCC.Select(x => new MailboxAddress(x)));

            email.Subject = message.Subject;
            email.Body = new TextPart(GetFormat(message.Format))
            {
                Text = message.Body
            };

            return PerformSend();

            async Task PerformSend()
            {
                await _transport.ConnectAsync(_options.Server, _options.Port, _options.UseSSL);
                await _transport.AuthenticateAsync(_options.Username, _options.Password);

                await _transport.SendAsync(email);

                await _transport.DisconnectAsync(true);
            }
        }

        private TextFormat GetFormat(EmailFormat format)
        {
            if (format == EmailFormat.Html)
                return TextFormat.Html;

            if (format == EmailFormat.Text)
                return TextFormat.Text;

            if (format == EmailFormat.RichText)
                return TextFormat.RichText;

            throw new InvalidOperationException($"Email format {format} is not supported");
        }
    }
}
