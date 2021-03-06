﻿namespace Dxw.Throttling.Core.Rules
{
    using System;
    using System.Linq;
    using System.Xml;
    using System.Threading.Tasks;

    using Storages;
    using Keyers;
    using Processors;
    using Configuration;
    using Logging;

    public class StorageKeyerProcessorPassBlockRule : StorageKeyerProcessorRule<object, PassBlockVerdict> { }

    public class StorageKeyerProcessorRule<TArg, TRes> : IRule<TArg, TRes>, IXmlConfigurable<TArg, TRes>, INamed
    {
        private ILog _log;

        public IStorage Storage { get; set; }

        public IKeyer<TArg> Keyer { get; set; }

        public IProcessor<TArg, TRes> Processor { get; set; }

        public string Name { get; private set; }

        public IApplyResult<TRes> Apply(TArg context = default(TArg))
        {
            var key = Keyer.GetKey(context);

            var result = Processor.Process(key, context, Storage.GetStorePoint());

            var ruledResult = ApplyResult<TRes>.FromResultAndRule(result, this);

            return ruledResult;
        }

        public async Task<IApplyResult<TRes>> ApplyAsync(TArg context = default(TArg))
        {
            var key = Keyer.GetKey(context);

            var result = await Processor.ProcessAsync(key, context, Storage.GetStorePoint());

            var ruledResult = ApplyResult<TRes>.FromResultAndRule(result, this);

            return ruledResult;
        }

        public void Configure(XmlNode node, IConfiguration<TArg, TRes> context)
        {
            _log = context.Log;

            Name = node.Attributes["name"]?.Value;

            _log.Log(LogLevel.Debug, string.Format("Configuring rule '{0}' of type '{1}'", Name, GetType().FullName));

            Storage = CreateStorage(node, context);
            Keyer = CreateKeyer(node, context);
            Processor = CreateProcessor(node, context);
        }

        private IStorage CreateStorage(XmlNode node, IConfiguration<TArg, TRes> context)
        {
            var nStorage = node.SelectSingleNode("storage");
            var storageName = nStorage.Attributes["name"].Value;
            var storage = context.Storages.First(s => string.Equals(s.Name, storageName, System.StringComparison.InvariantCulture));
            return storage;
        }

        private IKeyer<TArg> CreateKeyer(XmlNode node, IConfiguration<TArg, TRes> context)
        {
            var nKeyer = node.SelectSingleNode("keyer");
            var typeName = nKeyer.Attributes["type"].Value;
            var type = Type.GetType(typeName);
            var keyer = (IKeyer<TArg>)Activator.CreateInstance(type);
            var configurableTyped = keyer as IXmlConfigurable<TArg, TRes>;
            if (configurableTyped != null)
            {
                configurableTyped.Configure(nKeyer, context);
            }
            else
            {
                var configurable = keyer as IXmlConfigurable;
                if (configurable != null)
                    configurable.Configure(node, context);
            }
            return keyer;
        }

        private IProcessor<TArg, TRes> CreateProcessor(XmlNode node, IConfiguration<TArg, TRes> context)
        {
            var nProcessor = node.SelectSingleNode("processor");
            var typeName = nProcessor.Attributes["type"].Value;
            var type = Type.GetType(typeName);
            var processor = (IProcessor<TArg, TRes>)Activator.CreateInstance(type);

            var configurableTyped = processor as IXmlConfigurable<TArg, TRes>;
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
