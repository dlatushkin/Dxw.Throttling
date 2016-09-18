namespace Dxw.Throttling.Asp.Rules
{
    using System;
    using System.Threading.Tasks;
    using Dxw.Throttling.Core.Rules;

    public class AspWhiteListRule : KeyListPassBlockRule<IAspArgs>
    {
        public override IApplyResult<PassBlockVerdict> Apply(IAspArgs context = null)
        {
            var key = Keyer.GetKey(context);

            if (HasKey(key))
            {
                return ApplyResultPassBlock.Pass(this);
            }
            else
            {
                return ApplyResultPassBlock.Block(this, "The request isn't whitelisted.");
            }
        }

        public override Task<IApplyResult<PassBlockVerdict>> ApplyAsync(IAspArgs context = null)
        {
            return Task.FromResult(Apply(context));
        }
    }
}
