namespace Dxw.Throttling.Core.Processors
{
    using Rules;
    using Dxw.Throttling.Core.Storages;

    public interface IProcessEventResult
    {
        IApplyResult Result { get; }
    }

    public struct ProcessEventResult : IProcessEventResult
    {
        public IApplyResult Result { get; set; }
    }
}
