namespace Dxw.Throttling.Core.Rules
{
    using Storage;
    using Keyer;
    using EventProcessor;
    using System;
    using System.Xml;

    public class StorageKeyerProcessorRule : IRule, IRequireStorage, IRequireKeyer, IRequireProcessor, IXmlConfigurableRule
    {
        public IStorage Storage { get; set; }

        public IKeyer Keyer { get; set; }

        public IEventProcessor Processor { get; set; }

        public IApplyResult Apply(object context = null, IStorage storage = null)
        {
            var key = Keyer.GetKey(context);
            var result = Storage.Upsert(key, context, Processor.Process);
            return result.Result;
        }

        public void Configure(XmlNode node)
        {
            
        }
    }
}
