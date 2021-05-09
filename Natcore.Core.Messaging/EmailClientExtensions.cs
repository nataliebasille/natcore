using Natcore.Core.Exceptions;
using System.Threading.Tasks;

namespace Natcore.Core.Messaging
{
    public static class EmailClientExtensions
    {
        public static Task SendTextAsync(this IEmailClient client, string from, string to, string subject, string message)
        {
            ThrowIf.Argument.IsNull(to, nameof(to));

            return SendTextAsync(client, from, new string[] { to }, subject, message);
        }

        public static Task SendTextAsync(this IEmailClient client, string from, string[] to, string subject, string message)
        {
            ThrowIf.Argument.IsNull(from, nameof(from));
            ThrowIf.Argument.IsNullOrEmpty(to, nameof(to));

            return client.SendAsync(message: new TextEmailMessage(from, to, Body: message)
            {
                Body = message,
                Subject = subject
            });
        }

        public static Task SendHTMLAsync(this IEmailClient client, string from, string to, string subject, HtmlPart message)
        {
            ThrowIf.Argument.IsNull(to, nameof(to));

            return SendHTMLAsync(client, from, new string[] { to }, subject, message);
        }

        public static Task SendHTMLAsync(this IEmailClient client, string from, string[] to, string subject, HtmlPart message)
        {
            ThrowIf.Argument.IsNull(from, nameof(from));
            ThrowIf.Argument.IsNullOrEmpty(to, nameof(to));

            return client.SendAsync(message: new HtmlEmailMessage(from, to, Body: message)
            {
                Subject = subject
            });
        }
    }
}
