using System;

namespace Throttling
{
    public interface IThrottlerStorage : IDisposable
    {
        ThrottlingSlotState Hit(ThrottlingSlotKey key, DateTime utcNow);
    }
}
