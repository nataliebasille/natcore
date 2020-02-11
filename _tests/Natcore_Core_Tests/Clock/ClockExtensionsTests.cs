using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Natcore.Core.Clock;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natcore_Core_Tests.Clock
{
    [TestClass]
    public class ClockExtensionsTests
    {
        [TestClass]
        public class CurrentLocalTimeMethod
        {
            [TestMethod]
            public void Converts_currenttime_to_local_time()
            {
                var clock = new TestClock()
                {
                    CurrentTime = new DateTimeOffset(2020, 2, 29, 5, 30, 0, new TimeSpan(-4, 0, 0))
                };

                clock.CurrentLocalTime(5)
                    .Should()
                    .Be(new DateTimeOffset(2020, 2, 29, 14, 30, 0, new TimeSpan(5, 0, 0)));
            }
        }

        public class TestClock : IClock
        {
            public DateTimeOffset CurrentTime { get; set; }
        }
    }
}
