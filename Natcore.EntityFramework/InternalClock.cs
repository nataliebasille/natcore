using Natcore.Core.Clock;
using System;

namespace Natcore.EntityFramework
{
    internal class InternalClock : IClock
    {
        public DateTimeOffset CurrentTime => DateTimeOffset.UtcNow;
    }
}
