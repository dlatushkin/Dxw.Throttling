namespace Dxw.Throttling.Core.EventProcessor
{
    using System;
    using Keyer;
    using Rules;

    public interface IProcessEventResult
    {
        object NewState { get; }
        IApplyResult Result { get; }
    }

    public struct ProcessEventResult : IProcessEventResult
    {
        public object NewState { get; set; }

        public IApplyResult Result { get; set; }
    }

    public interface IEventProcessor
    {
        IProcessEventResult Process(IRequestContext context = null, object prevState = null);
    }
}
