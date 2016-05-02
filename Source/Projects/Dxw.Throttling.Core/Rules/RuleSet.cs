namespace Dxw.Throttling.Core.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public abstract class RuleSet : IRule, IXmlConfigurable
    {
        public IEnumerable<IRule> Rules { get; set; }

        public bool CallEachRule { get; set; }

        public bool BlockResultsOnly { get; set; }

        public abstract IApplyResult Apply(object context = null);

        public RuleSet()
        {
            BlockResultsOnly = true;
        }

        public void Configure(XmlNode node)
        {
            throw new NotImplementedException();
        }
    }
}
