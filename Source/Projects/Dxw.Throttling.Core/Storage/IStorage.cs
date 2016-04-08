namespace Dxw.Throttling.Core.Storage
{
    using Dxw.Throttling.Core.Rules;

    public delegate object StorageUpsertFunc(object currentValue);

    public interface IStorage
    {
        object Upsert(object key, ISlotCalculator slotCalculator);
    }
}
