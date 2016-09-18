namespace Dxw.Throttling.Owin.Rules
{
    using System;
    using System.Threading.Tasks;
    using Dxw.Throttling.Core.Rules;

    public class OwinWhiteListRule : KeyListPassBlockRule<IOwinArgs>
    {
        public override IApplyResult<PassBlockVerdict> Apply(IOwinArgs context = null)
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

        public override Task<IApplyResult<PassBlockVerdict>> ApplyAsync(IOwinArgs context = null)
        {
            return Task.FromResult(Apply(context));
        }
    }
}
