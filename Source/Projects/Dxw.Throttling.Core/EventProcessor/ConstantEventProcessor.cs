namespace Dxw.Throttling.Core.EventProcessor
{
    using System;
    using Keyer;
    using Rules;
    

    public class ConstantEventProcessor : IEventProcessor
    {
        public bool Ok { get; set; }

        public IProcessEventResult Process(IRequestContext context = null, object prevState = null)
        {
            if (Ok)
                return new ProcessEventResult { Result = ApplyResult.Ok() };
            else
                return new ProcessEventResult { Result = ApplyResult.Error() };
        }
    }
}
