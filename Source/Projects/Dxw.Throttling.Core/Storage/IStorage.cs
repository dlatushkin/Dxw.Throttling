namespace Dxw.Throttling.Core.Storage
{
    using System;
    using Keyer;
    using EventProcessor;

    public delegate object StorageUpsertFunc(object currentValue);

    public interface IStorage
    {
        IProcessEventResult Upsert(object key, object context, Func<object, IStorageValue, IProcessEventResult> upsertFunc);
    }
}
