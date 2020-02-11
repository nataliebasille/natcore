using System;

namespace Natcore.Core.Clock
{
    public static class ClockExtensions
    {
        public static DateTimeOffset CurrentLocalTime(this IClock clock, decimal local_utc_offset)
            => clock.CurrentTime.ToOffset(new TimeSpan((long)(36000000000 * local_utc_offset)));

        public static DateTimeOffset CurrentUtcTime(this IClock clock)
            => clock.CurrentTime.ToUniversalTime();
    }
}
