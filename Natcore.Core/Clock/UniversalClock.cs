using System;

namespace Natcore.Core.Clock
{
    public class UniversalClock : IClock
    {
        public DateTimeOffset Time => DateTimeOffset.UtcNow;
    }
}
