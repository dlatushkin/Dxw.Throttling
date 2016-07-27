namespace Dxw.Throttling.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml;

    using Rules;
    using Storages;

    public class ConfigurationSectionHandler<T, TArg> : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var conf = new ThrottlingConfiguration<T, TArg>();

            var storagesSection = section.SelectSingleNode("storages");
            if (storagesSection != null)
                conf.Storages = CreateStorages(storagesSection, conf);

            var rulesSection = section.SelectSingleNode("rules");
            if (rulesSection != null)
                conf.Rules = CreateRules(rulesSection, conf);

            return conf;
        }

        private IList<IStorage> CreateStorages(XmlNode section, IConfiguration<T, TArg> context)
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

        private IEnumerable<IRule<T, TArg>> CreateRules(XmlNode section, IConfiguration<T, TArg> context)
        {
            var rules = new List<IRule<T, TArg>>();
            
            foreach (XmlNode nRule in section.ChildNodes)
            {
                var typeName = nRule.Attributes["type"].Value;
                var type = Type.GetType(typeName);
                var rule = (IRule<T, TArg>)Activator.CreateInstance(type);

                var configurableTyped = rule as IXmlConfigurable<T, TArg>;
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
