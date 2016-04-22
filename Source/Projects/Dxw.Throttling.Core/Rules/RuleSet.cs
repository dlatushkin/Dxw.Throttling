namespace Dxw.Throttling.Core.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Dxw.Throttling.Core.Keyer;
    using Dxw.Throttling.Core.Storage;

    public abstract class RuleSet : IRule, IXmlConfigurableRule
    {
        public List<IRule> Rules { get; set; }

        public bool CallEachRule { get; set; }

        public bool BlockResultsOnly { get; set; }

        public abstract IApplyResult Apply(IRequestContext context = null, IStorage storage = null);

        public void Configure(XmlNode node)
        {
            throw new NotImplementedException();
        }
    }
}
