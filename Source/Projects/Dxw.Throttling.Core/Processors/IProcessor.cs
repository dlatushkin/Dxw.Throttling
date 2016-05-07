namespace Dxw.Throttling.Core.Processors
{
    using Rules;
    using Storages;

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

    public interface IProcessor
    {
        IProcessEventResult Process(object context = null, IStorage storage = null, IStorageValue prevState = null, IRule rule = null);
    }
}
