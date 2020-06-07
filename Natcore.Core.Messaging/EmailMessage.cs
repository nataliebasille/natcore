﻿using System;
using System.Collections.Generic;

namespace Natcore.Core.Messaging
{
    public class EmailMessage
    {
        public string From { get; set; }

        public string[] To { get; set; }

        public string[] CC { get; set; }

        public string[] BCC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public EmailFormat Format { get; set; }

        public List<LinkedResource> Resources { get; } = new List<LinkedResource>();
    }
}
