namespace Dxw.Throttling.Core.EventProcessor
{
    using System;
    using System.Xml;
    using Configuration;
    using Keyer;
    using Rules;
    using Storage;

    public class ConstantEventProcessor : IEventProcessor, IXmlConfigurable
    {
        public bool Ok { get; set; }

        public IProcessEventResult Process(object context = null, IStorageValue prevState = null, IRule rule = null)
        {
            if (Ok)
                return new ProcessEventResult { Result = ApplyResult.Ok(rule) };
            else
                return new ProcessEventResult { Result = ApplyResult.Error(rule) };
        }

        public void Configure(XmlNode node, IConfiguratedRules context)
        {
            var okAttr = node.Attributes["Ok"];
            if (okAttr == null) return;

            bool ok;
            bool.TryParse(okAttr.Value, out ok);
            Ok = ok;
        }
    }
}
