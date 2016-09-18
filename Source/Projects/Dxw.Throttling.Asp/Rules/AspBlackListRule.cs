namespace Dxw.Throttling.Asp.Rules
{
    using System.Threading.Tasks;
    using Dxw.Throttling.Core.Rules;

    public class AspBlackListRule : KeyListPassBlockRule<IAspArgs>
    {
        public override IApplyResult<PassBlockVerdict> Apply(IAspArgs context = null)
        {
            var key = Keyer.GetKey(context);

            if (HasKey(key))
            {
                return ApplyResultPassBlock.Block(this, "The request is black-listed.");
            }
            else
            {
                return ApplyResultPassBlock.Pass(this);
            }
        }

        public override Task<IApplyResult<PassBlockVerdict>> ApplyAsync(IAspArgs context = null)
        {
            return Task.FromResult(Apply(context));
        }
    }
}
