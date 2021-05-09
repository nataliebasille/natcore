using System.Collections.Generic;

namespace Natcore.Core.Messaging
{
    public abstract record EmailMessage(string From, string[] To)
    {
        public string[] CC { get; init; }

        public string[] BCC { get; init; }

        public string Subject { get; init; }

        public List<LinkedResource> Resources { get; } = new List<LinkedResource>();
    }

    public record TextEmailMessage(string From, string[] To, string Body) : EmailMessage(From, To)
    { }

    public record HtmlEmailMessage(string From, string[] To, HtmlPart Body) : EmailMessage(From, To) 
    { }
}
