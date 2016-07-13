namespace Dxw.Throttling.Core.Processors
{
    using System.Xml;
    using Configuration;
    using Rules;
    using Storages;
    using System;

    public class ConstantEventProcessor<T> : IProcessor<T>, IXmlConfigurable
    {
        public T Value { get; set; }

        public IApplyResult<T> Process(object key, object context = null, object storeEndpoint = null/*, IRule<T> rule = null*/)
        {
            return new ApplyResult<T> { Verdict = Value };
            //if (Ok)
            //    return ApplyResultPassBlock.Pass(rule);
            //else
            //    return ApplyResultPassBlock.Block(rule);
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            var okAttr = node.Attributes["Ok"];
            if (okAttr == null) return;

            Value = (T)Convert.ChangeType(okAttr, typeof(T));
        }
    }
}
