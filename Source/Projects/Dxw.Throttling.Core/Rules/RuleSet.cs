namespace Dxw.Throttling.Core.Rules
{
    using Configuration;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public abstract class RuleSet<TArg, TRes> : IRule<TArg, TRes>, IXmlConfigurable<TArg, TRes>
    {
        public IEnumerable<IRule<TArg, TRes>> Rules { get; set; }

        public bool CallEachRule { get; set; }

        public bool BlockResultsOnly { get; set; }

        public string Name { get { return GetType().Name; } }

        public abstract IApplyResult<TRes> Apply(TArg context = default(TArg));

        public RuleSet()
        {
            BlockResultsOnly = true;
        }

        public void Configure(XmlNode node, IConfiguration<TArg, TRes> context)
        {
            var rules = new List<IRule<TArg, TRes>>();

            foreach (XmlNode nRule in node.SelectSingleNode("rules").ChildNodes)
            {
                var typeName = nRule.Attributes["type"].Value;
                var type = Type.GetType(typeName);
                var rule = (IRule<TArg, TRes>)Activator.CreateInstance(type);
                var configurable = rule as IXmlConfigurable;
                if (configurable != null)
                    configurable.Configure(nRule, context);
                rules.Add(rule);
            }

            Rules = rules;
        }
    }
}
