namespace Dxw.Throttling.Core.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml;

    using Rules;
    using Storage;

    public class ThrottleConfiguration : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var configuratedRules = new ConfiguratedRules();

            var storagesSection = section.SelectSingleNode("storages");
            if (storagesSection != null)
                configuratedRules.Storages = CreateStorages(storagesSection, configuratedRules);

            var rulesSection = section.SelectSingleNode("rules");
            if (rulesSection != null)
                configuratedRules.Rules = CreateRules(rulesSection, configuratedRules);

            return configuratedRules;
        }

        private IList<IStorage> CreateStorages(XmlNode section, IConfiguratedRules context)
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

        private IEnumerable<IRule> CreateRules(XmlNode section, IConfiguratedRules context)
        {
            var rules = new List<IRule>();

            foreach (XmlNode nRule in section.ChildNodes)
            {
                var typeName = nRule.Attributes["type"].Value;
                var type = Type.GetType(typeName);
                var rule = (IRule)Activator.CreateInstance(type);
                var configurable = rule as IXmlConfigurable;
                if (configurable != null)
                    configurable.Configure(nRule, context);
                rules.Add(rule);
            }

            return rules;
        }
    }
}
