namespace Dxw.Throttling.Core.Rules
{
    using Dxw.Throttling.Core.Keyer;
    using Storage;

    public interface ISlotCalculator
    {
        IStorageSlot Create();

        IStorageSlot Update();

        IApplyResult CalculateResult(object slot);
    }
}
