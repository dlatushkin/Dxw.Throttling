namespace Dxw.Throttling.Core.Processors
{
    using System.Xml;
    using Configuration;
    using Rules;
    using Storages;
    using System;

    public class ConstantEventProcessor<TRes> : IProcessor<TRes, object>, IXmlConfigurable
    {
        public TRes Value { get; set; }

        public IApplyResult<TRes> Process(object key, object context = null, object storeEndpoint = null)
        {
            return new ApplyResult<TRes> { Verdict = Value };
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            var okAttr = node.Attributes["Ok"];
            if (okAttr == null) return;

            Value = (TRes)Convert.ChangeType(okAttr, typeof(TRes));
        }
    }
}
