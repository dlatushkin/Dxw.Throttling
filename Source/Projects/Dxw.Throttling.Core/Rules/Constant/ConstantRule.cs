﻿namespace Dxw.Throttling.Core.Rules.Constant
{
    public class ConstantRule<TArg, TRes> : IRule<TArg, TRes>
    {
        public TRes Value { get; set; }

        public virtual string Name { get { return this.GetType().Name; } }

        public IApplyResult<TRes> Apply(TArg context = default(TArg))
        {
            return new ApplyResult<TRes>
            {
                Rule = this,
                Verdict = Value,
                Reason = new ApplyError { Message = "Always equals " + Value }
            };
        }
    }
}
