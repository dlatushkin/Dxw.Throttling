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

    public class StorageKeyerProcessorPassBlockRule : StorageKeyerProcessorRule<PassBlockVerdict, object> { }

    public class StorageKeyerProcessorRule<T, TArg> : IRule<T, TArg>, IXmlConfigurable<T, TArg>, INamed//, IRequireProcessor<T>, IRequireStorage, IRequireKeyer
    {
        public IStorage Storage { get; set; }

        public IKeyer<TArg> Keyer { get; set; }

        public IProcessor<T> Processor { get; set; }

        public string Name { get; private set; }

        public IApplyResult<T> Apply(TArg context = default(TArg))
        {
            var key = Keyer.GetKey(context);

            var result = Processor.Process(key, context, Storage.GetStorePoint());

            var ruledResult = ApplyResult<T>.FromResultAndRule(result, this);

            return ruledResult;
        }

        public void Configure(XmlNode node, IConfiguration<T, TArg> context)
        {
            Name = node.Attributes["name"]?.Value;

            Storage = CreateStorage(node, context);
            Keyer = CreateKeyer(node, context);
            Processor = CreateProcessor(node, context);
        }

        private IStorage CreateStorage(XmlNode node, IConfiguration<T, TArg> context)
        {
            var nStorage = node.SelectSingleNode("storage");
            var storageName = nStorage.Attributes["name"].Value;
            var storage = context.Storages.First(s => string.Equals(s.Name, storageName, System.StringComparison.InvariantCulture));
            return storage;
        }

        private IKeyer<TArg> CreateKeyer(XmlNode node, IConfiguration<T, TArg> context)
        {
            var nKeyer = node.SelectSingleNode("keyer");
            var typeName = nKeyer.Attributes["type"].Value;
            var type = Type.GetType(typeName);
            var keyer = (IKeyer<TArg>)Activator.CreateInstance(type);
            var configurable = keyer as IXmlConfigurable<T, TArg>;
            if (configurable != null)
                configurable.Configure(nKeyer, context);
            return keyer;
        }

        private IProcessor<T> CreateProcessor(XmlNode node, IConfiguration<T, TArg> context)
        {
            var nProcessor = node.SelectSingleNode("processor");
            var typeName = nProcessor.Attributes["type"].Value;
            var type = Type.GetType(typeName);
            var processor = (IProcessor<T>)Activator.CreateInstance(type);

            var configurableTyped = processor as IXmlConfigurable<T, TArg>;
            if (configurableTyped != null)
            {
                configurableTyped.Configure(nProcessor, context);
            }
            else
            {
                var configurable = processor as IXmlConfigurable;
                if (configurable != null)
                    configurable.Configure(nProcessor, context);
            }

            return processor;
        }
    }
}
