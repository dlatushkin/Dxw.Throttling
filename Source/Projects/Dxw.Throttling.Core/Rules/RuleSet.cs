﻿namespace Dxw.Throttling.Core.Rules
{
    using Configuration;
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Threading.Tasks;

    public abstract class RuleSet<TArg, TRes> : IRule<TArg, TRes>, IXmlConfigurable<TArg, TRes>
    {
        public IEnumerable<IRule<TArg, TRes>> Rules { get; set; }

        public bool CallEachRule { get; set; }

        public bool BlockResultsOnly { get; set; }

        public string Name { get; private set; }

        public abstract IApplyResult<TRes> Apply(TArg context = default(TArg));

        public abstract Task<IApplyResult<TRes>> ApplyAsync(TArg context = default(TArg));

        public RuleSet()
        {
            BlockResultsOnly = true;
        }

        public void Configure(XmlNode node, IConfiguration<TArg, TRes> context)
        {
            Name = node.Attributes["name"]?.Value;

            var rules = new List<IRule<TArg, TRes>>();

            foreach (XmlNode nRule in node.SelectSingleNode("rules").ChildNodes)
            {
                if (nRule.NodeType != XmlNodeType.Element) continue;

                var typeName = nRule.Attributes["type"].Value;
                var type = Type.GetType(typeName);
                var rule = (IRule<TArg, TRes>)Activator.CreateInstance(type);

                var configurableTyped = rule as IXmlConfigurable<TArg, TRes>;
                if (configurableTyped != null)
                {
                    configurableTyped.Configure(nRule, context);
                }
                else
                {
                    var configurable = rule as IXmlConfigurable;
                    if (configurable != null)
                        configurable.Configure(nRule, context);
                }

                rules.Add(rule);
            }

            Rules = rules;
        }
    }
}
