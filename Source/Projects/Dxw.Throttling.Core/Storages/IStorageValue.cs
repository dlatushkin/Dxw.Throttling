namespace Dxw.Throttling.Core.Storages
{
    using System;

    public interface IStorageValue<out T>
    {
        bool IsExpired(DateTime utcNow);
        T Value { get; }
    }

    public interface IStorageValue : IStorageValue<object> { }
}
