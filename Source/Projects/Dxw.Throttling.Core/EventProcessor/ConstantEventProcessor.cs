namespace Dxw.Throttling.Core.EventProcessor
{
    using System;
    using Keyer;
    using Rules;
    using Storage;

    public class ConstantEventProcessor : IEventProcessor
    {
        public bool Ok { get; set; }

        public IProcessEventResult Process(object context = null, IStorageValue prevState = null)
        {
            if (Ok)
                return new ProcessEventResult { Result = ApplyResult.Ok() };
            else
                return new ProcessEventResult { Result = ApplyResult.Error() };
        }
    }
}
