namespace Dxw.Throttling.Core.Rules
{
    using System;
    using Dxw.Throttling.Core.Keyer;
    using Dxw.Throttling.Core.Storage;

    public class RuleAndNode : RuleSet
    {
        public override IApplyResult Apply(IRequestContext context = null, IStorage storage = null)
        {
            foreach (var rule in Rules)
            {
                var result = rule.Apply(context, storage);
            }

            throw new NotImplementedException();
        }
    }
}
