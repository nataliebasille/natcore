using System;
using System.Collections.Generic;
using System.Text;

namespace Natcore.Core.Upload
{
    public class File
    {
        public string Name { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}
