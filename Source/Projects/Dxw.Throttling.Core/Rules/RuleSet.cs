namespace Dxw.Throttling.Core.Rules
{
    using Configuration;
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

        public void Configure(XmlNode node, IConfiguration context)
        {
            var rules = new List<IRule>();

            foreach (XmlNode nRule in node.ChildNodes)
            {
                var typeName = nRule.Attributes["type"].Value;
                var type = Type.GetType(typeName);
                var rule = (IRule)Activator.CreateInstance(type);
                var configurable = rule as IXmlConfigurable;
                if (configurable != null)
                    configurable.Configure(nRule, context);
                rules.Add(rule);
            }

            Rules = rules;
        }
    }
}
