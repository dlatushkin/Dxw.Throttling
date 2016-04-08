namespace Dxw.Throttling.Core.EventProcessor
{
    using Keyer;
    using Rules;
    using Storage;

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

    public interface IEventProcessor
    {
        IProcessEventResult Process(IRequestContext context = null, IStorageValue prevState = null);
    }
}
