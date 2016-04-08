namespace Dxw.Throttling.Core.Storage
{
    using System;
    using Keyer;
    using EventProcessor;

    public delegate object StorageUpsertFunc(object currentValue);

    public interface IStorage
    {
        //object Upsert(object key, ISlotCalculator slotCalculator);
        IProcessEventResult Upsert(object key, Func<IProcessEventResult, IRequestContext, object> upsertFunc);
    }
}
