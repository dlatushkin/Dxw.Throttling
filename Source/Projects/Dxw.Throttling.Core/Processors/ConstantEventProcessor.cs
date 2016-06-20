namespace Dxw.Throttling.Core.Processors
{
    using System.Xml;
    using Configuration;
    using Rules;
    using Storages;

    public class ConstantEventProcessor : IProcessor, IXmlConfigurable
    {
        public bool Ok { get; set; }

        public IApplyResult Process(object key, object context = null, object storeEndpoint = null, IRule rule = null)
        {
            if (Ok)
                return ApplyResult.Ok(rule);
            else
                return ApplyResult.Error(rule);
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            var okAttr = node.Attributes["Ok"];
            if (okAttr == null) return;

            bool ok;
            bool.TryParse(okAttr.Value, out ok);
            Ok = ok;
        }
    }
}
