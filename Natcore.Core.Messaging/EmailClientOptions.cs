using System;
using System.Collections.Generic;
using System.Text;

namespace Natcore.Core.Messaging
{
    public class EmailClientOptions
    {
        public static IEmailClientAuthentication Credentials(string username, string password) => new EmailClientCredentialAuthentication(username, password);

        public string Server { get; set; }

        public int Port { get; set; }

        public IEmailClientAuthentication Authentication { get; set; }

        public bool UseSSL { get; set; }
    }
}
