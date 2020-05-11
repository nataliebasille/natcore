using Natcore.Core.Exceptions;
using System.Threading.Tasks;

namespace Natcore.Core.Messaging
{
    public static class EmailClientExtensions
    {
        public static Task SendText(this IEmailClient client, string from, string to, string subject, string message)
        {
            ThrowIf.Argument.IsNull(to, nameof(to));

            return SendText(client, from, new string[] { to }, subject, message);
        }

        public static Task SendText(this IEmailClient client, string from, string[] to, string subject, string message)
        {
            ThrowIf.Argument.IsNull(from, nameof(from));
            ThrowIf.Argument.IsNullOrEmpty(to, nameof(to));

            return client.Send(new EmailMessage
            {
                Format = EmailFormat.Text,
                From = from,
                To = to,
                Body = message,
                Subject = subject
            });
        }

        public static Task SendHTML(this IEmailClient client, string from, string to, string subject, string message)
        {
            ThrowIf.Argument.IsNull(to, nameof(to));

            return SendHTML(client, from, new string[] { to }, subject, message);
        }

        public static Task SendHTML(this IEmailClient client, string from, string[] to, string subject, string message)
        {
            ThrowIf.Argument.IsNull(from, nameof(from));
            ThrowIf.Argument.IsNullOrEmpty(to, nameof(to));

            return client.Send(new EmailMessage
            {
                Format = EmailFormat.Html,
                From = from,
                To = to,
                Body = message,
                Subject = subject
            });
        }
    }
}
