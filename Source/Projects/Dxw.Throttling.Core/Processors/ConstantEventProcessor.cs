﻿namespace Dxw.Throttling.Core.Processors
{
    using System.Xml;
    using Configuration;
    using Rules;
    using Storages;

    public class ConstantEventProcessor : IProcessor, IXmlConfigurable
    {
        public bool Ok { get; set; }

        public IProcessEventResult Process(object key, object context = null, object storeEndpoint = null, IRule rule = null)
        {
            if (Ok)
                return new ProcessEventResult { Result = ApplyResult.Ok(rule) };
            else
                return new ProcessEventResult { Result = ApplyResult.Error(rule) };
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
