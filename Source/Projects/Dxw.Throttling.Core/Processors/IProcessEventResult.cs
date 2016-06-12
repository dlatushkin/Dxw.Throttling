namespace Dxw.Throttling.Core.Processors
{
    using Rules;
    using Dxw.Throttling.Core.Storages;

    public interface IProcessEventResult
    {
        IStorageValue NewState { get; }
        IApplyResult Result { get; }
    }

    public struct ProcessEventResult : IProcessEventResult
    {
        public IStorageValue NewState { get; set; }

        public IApplyResult Result { get; set; }
    }
}
