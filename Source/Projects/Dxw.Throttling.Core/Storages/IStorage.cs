namespace Dxw.Throttling.Core.Storages
{
    using System;

    using Processors;
    using Rules;

    public delegate object StorageUpsertFunc(object currentValue);

    public interface IStorage
    {
        IProcessEventResult Upsert(object key, object context, IRule rule, Func<object, IStorageValue, IRule, IProcessEventResult> upsertFunc);

        string Name { get; }
    }
}
