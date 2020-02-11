using System;

namespace Natcore.Core.Clock
{
    public interface IClock
    {
        DateTimeOffset Time { get; }
    }
}
