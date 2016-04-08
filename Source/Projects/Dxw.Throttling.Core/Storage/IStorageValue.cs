namespace Dxw.Throttling.Core.Storage
{
    using System;

    public interface IStorageValue
    {
        bool IsExpired(DateTime utcNow);
        object Value { get; }
    }
}
