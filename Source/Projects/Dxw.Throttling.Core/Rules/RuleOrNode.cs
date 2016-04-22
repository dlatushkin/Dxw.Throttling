namespace Dxw.Throttling.Core.Rules
{
    using System;
    using Dxw.Throttling.Core.Keyer;
    using Dxw.Throttling.Core.Storage;

    public class RuleOrNode : IRule
    {
        public IApplyResult Apply(IRequestContext context = null, IStorage storage = null)
        {
            throw new NotImplementedException();
        }
    }
}
