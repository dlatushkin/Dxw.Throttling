namespace Dxw.Throttling.Core.Storages
{
    using System;

    public interface IStorageValue
    {
        bool IsExpired(DateTime utcNow);
        object Value { get; }
    }
}
