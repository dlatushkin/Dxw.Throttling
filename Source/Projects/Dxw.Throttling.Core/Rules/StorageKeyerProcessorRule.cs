namespace Dxw.Throttling.Core.Rules
{
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

            var nStorage = node.SelectSingleNode("storage");
            var storageName = nStorage.Attributes["name"].Value;
            Storage = context.Storages.First(s => string.Equals(s.Name,  storageName, System.StringComparison.InvariantCulture));

            var nKeyer = node.SelectSingleNode("keyer");
            var nProcessor = node.SelectSingleNode("processor");
        }
    }
}
