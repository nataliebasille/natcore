using System;
using System.Collections.Generic;
using System.Text;

namespace Natcore.Core.Rest
{
    public interface ICookie
    {
        public string Key { get; }

        public string Value { get; }

        public bool HttpOnly { get; }

        public string SameSite { get; }

        public bool Secure { get; }
    }
}
