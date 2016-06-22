namespace Dxw.Throttling.Core.Rules
{
    using System;

    public class TooLateTooEarlyRule : IRule
    {
        public int Times { get; set; }
        public TimeSpan PerPeriod { get; set; }

        public IApplyResult Apply(object context = null)
        {
            var now = DateTime.Now;

            if (now.TimeOfDay > new TimeSpan(18, 0, 0))
                return ApplyResultPassBlock.Block(msg: "It's too late now");

            if (now.TimeOfDay < new TimeSpan(6, 0, 0))
                return ApplyResultPassBlock.Block(msg: "It's too early now");

            return ApplyResultPassBlock.Pass(this);
        }
    }
}
