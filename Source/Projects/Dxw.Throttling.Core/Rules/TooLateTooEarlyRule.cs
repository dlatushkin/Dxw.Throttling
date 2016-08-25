namespace Dxw.Throttling.Core.Rules
{
    using System;

    public class TooLateTooEarlyRule : IRule<DateTime, PassBlockVerdict>
    {
        public virtual string Name { get { return this.GetType().Name; } }

        public IApplyResult<PassBlockVerdict> Apply(DateTime now = default(DateTime))
        {
            if (now.TimeOfDay > new TimeSpan(18, 0, 0))
                return ApplyResultPassBlock.Block(msg: "It's too late now");

            if (now.TimeOfDay < new TimeSpan(6, 0, 0))
                return ApplyResultPassBlock.Block(msg: "It's too early now");

            return ApplyResultPassBlock.Pass(this);
        }
    }
}

