using System;

namespace Natcore.Core.Rest
{
    public class Cookie : ICookie
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string Path { get; set; }

        public bool HttpOnly { get; set; }

        public string SameSite { get; set; }

        public bool Secure { get; set; }
    }
}
