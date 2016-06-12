namespace Dxw.Throttling.Core.Rules
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Xml;

    using Storages;
    using Keyers;
    using Processors;
    using Configuration;
    using Exceptions;

    public class StorageKeyerProcessorRule : IRule, IRequireStorage, IRequireKeyer, IRequireProcessor, IXmlConfigurable, INamed
    {
        public IStorage Storage { get; set; }

        public IKeyer Keyer { get; set; }

        public IProcessor Processor { get; set; }

        public string Name { get; private set; }

        public IApplyResult Apply(object context = null)
        {
            var key = Keyer.GetKey(context);

            var result = Processor.Process(key, Storage.GetStorePoint(), context, this);

            return result.Result;
        }

        private IStorageValue AddValue(object obj)
        {
            throw new NotImplementedException();
        }

        private IStorageValue UpdateValue(object obj, IStorageValue curValue)
        {
            throw new NotImplementedException();
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            Name = node.Attributes["name"]?.Value;

            Storage = CreateStorage(node, context);
            Keyer = CreateKeyer(node, context);
            Processor = CreateProcessor(node, context);
        }

        private IStorage CreateStorage(XmlNode node, IConfiguration context)
        {
            var nStorage = node.SelectSingleNode("storage");
            var storageName = nStorage.Attributes["name"].Value;
            var storage = context.Storages.First(s => string.Equals(s.Name, storageName, System.StringComparison.InvariantCulture));
            return storage;
        }

        private IKeyer CreateKeyer(XmlNode node, IConfiguration context)
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

        private IProcessor CreateProcessor(XmlNode node, IConfiguration context)
        {
            var nProcessor = node.SelectSingleNode("processor");
            var typeName = nProcessor.Attributes["type"].Value;
            var type = Type.GetType(typeName);
            var processor = (IProcessor)Activator.CreateInstance(type);
            var configurable = processor as IXmlConfigurable;
            if (configurable != null)
                configurable.Configure(nProcessor, context);
            return processor;
        }
    }
}
