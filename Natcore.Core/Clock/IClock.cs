using System;

namespace Natcore.Core.Clock
{
    public interface IClock
    {
        DateTimeOffset CurrentTime { get; }
    }
}
