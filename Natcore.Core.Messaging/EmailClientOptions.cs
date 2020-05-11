using System;
using System.Collections.Generic;
using System.Text;

namespace Natcore.Core.Messaging
{
    public class EmailClientOptions
    {
        public string Server { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool UseSSL { get; set; }
    }
}
