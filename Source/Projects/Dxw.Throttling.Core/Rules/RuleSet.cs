namespace Dxw.Throttling.Core.Rules
{
    using Configuration;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public abstract class RuleSet<T> : IRule<T>, IXmlConfigurable<T>
    {
        public IEnumerable<IRule<T>> Rules { get; set; }

        public bool CallEachRule { get; set; }

        public bool BlockResultsOnly { get; set; }

        public abstract IApplyResult<T> Apply(object context = null);

        public RuleSet()
        {
            BlockResultsOnly = true;
        }

        public void Configure(XmlNode node, IConfiguration<T> context)
        {
            var rules = new List<IRule<T>>();

            foreach (XmlNode nRule in node.ChildNodes)
            {
                var typeName = nRule.Attributes["type"].Value;
                var type = Type.GetType(typeName);
                var rule = (IRule<T>)Activator.CreateInstance(type);
                var configurable = rule as IXmlConfigurable;
                if (configurable != null)
                    configurable.Configure(nRule, context);
                rules.Add(rule);
            }

            Rules = rules;
        }
    }
}
