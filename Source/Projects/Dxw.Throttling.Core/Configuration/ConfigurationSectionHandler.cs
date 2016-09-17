namespace Dxw.Throttling.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml;

    using Rules;
    using Storages;
    using Logging;

    public class ConfigurationSectionHandler<TArg, TRes> : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var conf = new ThrottlingConfiguration<TArg, TRes>();

            var logNode = section.SelectSingleNode("log");
            if (logNode != null)
                conf.Log = CreateLog(logNode, conf);

            var storagesSection = section.SelectSingleNode("storages");
            if (storagesSection != null)
                conf.Storages = CreateStorages(storagesSection, conf);

            var rulesSection = section.SelectSingleNode("rules");
            if (rulesSection != null)
                conf.Rules = CreateRules(rulesSection, conf);

            return conf;
        }

        private ILog CreateLog(XmlNode node, IConfiguration<TArg, TRes> context)
        {
            var typeName = node.Attributes["type"].Value;
            var type = Type.GetType(typeName);
            var log = (ILog)Activator.CreateInstance(type);
            var configurable = log as IXmlConfigurable;
            if (configurable != null) configurable.Configure(node, context);
            return log;
        }

        private IList<IStorage> CreateStorages(XmlNode section, IConfiguration<TArg, TRes> context)
        {
            var storages = new List<IStorage>();

            foreach (XmlNode nStorage in section.ChildNodes)
            {
                var typeName = nStorage.Attributes["type"].Value;
                var type = Type.GetType(typeName);
                var storage = (IStorage)Activator.CreateInstance(type);
                var configurable = storage as IXmlConfigurable;
                if (configurable != null)
                    configurable.Configure(nStorage, context);
                storages.Add(storage);
            }

            return storages;
        }

        private IEnumerable<IRule<TArg, TRes>> CreateRules(XmlNode section, IConfiguration<TArg, TRes> context)
        {
            var rules = new List<IRule<TArg, TRes>>();
            
            foreach (XmlNode nRule in section.ChildNodes)
            {
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
    
            return rules;
        }
    }
}
