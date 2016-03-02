namespace Dxw.Throttling.Core
{
    using System;

    public interface IThrottlerStorage : IDisposable
    {
        ThrottlingSlotState Hit(ThrottlingSlotKey key, DateTime utcNow);
    }
}
