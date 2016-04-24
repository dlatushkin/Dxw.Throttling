namespace Dxw.Throttling.Core.Rules
{
    using Storage;
    using Keyer;
    using System;

    public class TooLateTooEarlyRule : IRule
    {
        public int Times { get; set; }
        public TimeSpan PerPeriod { get; set; }

        public IApplyResult Apply(object context = null, IStorage storage = null)
        {
            var now = DateTime.Now;

            if (now.TimeOfDay > new TimeSpan(18, 0, 0))
                return ApplyResult.Error("It's too late now");

            if (now.TimeOfDay < new TimeSpan(6, 0, 0))
                return ApplyResult.Error("It's too early now");

            return new ApplyResult { Block = false };
        }
    }
}
