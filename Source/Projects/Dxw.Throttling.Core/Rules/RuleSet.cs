namespace Dxw.Throttling.Core.Rules
{
    using Configuration;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public abstract class RuleSet<T, TArg> : IRule<T, TArg>, IXmlConfigurable<T, TArg>
    {
        public IEnumerable<IRule<T, TArg>> Rules { get; set; }

        public bool CallEachRule { get; set; }

        public bool BlockResultsOnly { get; set; }

        public string Name { get { return GetType().Name; } }

        public abstract IApplyResult<T> Apply(TArg context = default(TArg));

        public RuleSet()
        {
            BlockResultsOnly = true;
        }

        public void Configure(XmlNode node, IConfiguration<T, TArg> context)
        {
            var rules = new List<IRule<T, TArg>>();

            foreach (XmlNode nRule in node.ChildNodes)
            {
                var typeName = nRule.Attributes["type"].Value;
                var type = Type.GetType(typeName);
                var rule = (IRule<T, TArg>)Activator.CreateInstance(type);
                var configurable = rule as IXmlConfigurable;
                if (configurable != null)
                    configurable.Configure(nRule, context);
                rules.Add(rule);
            }

            Rules = rules;
        }
    }
}
