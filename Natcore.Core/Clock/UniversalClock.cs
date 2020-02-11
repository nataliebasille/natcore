using System;

namespace Natcore.Core.Clock
{
    public class UniversalClock : IClock
    {
        public DateTimeOffset CurrentTime => DateTimeOffset.UtcNow;
    }
}
