namespace Dxw.Throttling.Core.Storage
{
    using System;

    using Processor;
    using Rules;

    public delegate object StorageUpsertFunc(object currentValue);

    public interface IStorage
    {
        IProcessEventResult Upsert(object key, object context, IRule rule, Func<object, IStorageValue, IRule, IProcessEventResult> upsertFunc);

        string Name { get; }
    }
}
