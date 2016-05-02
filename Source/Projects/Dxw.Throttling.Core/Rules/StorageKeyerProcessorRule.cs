namespace Dxw.Throttling.Core.Rules
{
    using System;
    using System.Xml;
    using System.Linq;

    using Storage;
    using Keyer;
    using EventProcessor;
    using Configuration;

    public class StorageKeyerProcessorRule : IRule, IRequireStorage, IRequireKeyer, IRequireProcessor, IXmlConfigurable, INamed
    {
        public IStorage Storage { get; set; }

        public IKeyer Keyer { get; set; }

        public IEventProcessor Processor { get; set; }

        public string Name { get; private set; }

        public IApplyResult Apply(object context = null)
        {
            var key = Keyer.GetKey(context);
            var result = Storage.Upsert(key, context, this, Processor.Process);
            return result.Result;
        }

        public void Configure(XmlNode node, IConfiguratedRules context)
        {
            Name = node.Attributes["name"]?.Value;

            Storage = CreateStorage(node, context);
            Keyer = CreateKeyer(node, context);
            Processor = CreateProcessor(node, context);
        }

        private IStorage CreateStorage(XmlNode node, IConfiguratedRules context)
        {
            var nStorage = node.SelectSingleNode("storage");
            var storageName = nStorage.Attributes["name"].Value;
            var storage = context.Storages.First(s => string.Equals(s.Name, storageName, System.StringComparison.InvariantCulture));
            return storage;
        }

        private IKeyer CreateKeyer(XmlNode node, IConfiguratedRules context)
        {
            var nKeyer = node.SelectSingleNode("keyer");
            var typeName = nKeyer.Attributes["type"].Value;
            var type = Type.GetType(typeName);
            var keyer = (IKeyer)Activator.CreateInstance(type);
            var configurable = keyer as IXmlConfigurable;
            if (configurable != null)
                configurable.Configure(nKeyer, context);
            return keyer;
        }

        private IEventProcessor CreateProcessor(XmlNode node, IConfiguratedRules context)
        {
            var nProcessor = node.SelectSingleNode("processor");
            var typeName = nProcessor.Attributes["type"].Value;
            var type = Type.GetType(typeName);
            var processor = (IEventProcessor)Activator.CreateInstance(type);
            var configurable = processor as IXmlConfigurable;
            if (configurable != null)
                configurable.Configure(nProcessor, context);
            return processor;
        }
    }
}
