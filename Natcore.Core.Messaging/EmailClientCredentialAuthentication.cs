using MailKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natcore.Core.Messaging
{
    internal class EmailClientCredentialAuthentication : IEmailClientAuthentication
    {
        private readonly string _username;
        private readonly string _password;

        public EmailClientCredentialAuthentication(string username, string password)
        {
            _username = username;
            _password = password;
        }

        Task IEmailClientAuthentication.AuthenticateAsync(IMailTransport transport)
        {
            return transport.AuthenticateAsync(_username, _password);
        }
    }
}
