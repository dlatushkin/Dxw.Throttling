namespace Dxw.Throttling.Core.Storages
{
    using System;

    using Processors;
    using Rules;

    public interface IStorage
    {
        //IProcessEventResult Upsert(object key, object context, IRule rule, Func<object, IStorage, IStorageValue, IRule, IProcessEventResult> upsertFunc);

        /// <summary>
        /// Returns object that is used for load/store operations (e.g. ConcurrentDictionary, Redis Connection multiplexer etc.).
        /// </summary>
        /// <returns></returns>
        object GetStorePoint();

        string Name { get; }
    }
}
