namespace Dxw.Throttling.Core.Rules.Constant
{
    using System;
    using System.Xml;
    using Dxw.Throttling.Core.Configuration;

    public class ConstantPassBlockRule<TArg> : ConstantRule<TArg, PassBlockVerdict>, IXmlConfigurable
    {
        public void Configure(XmlNode node, IConfiguration context)
        {
            var valAttr = node.Attributes["value"];
            if (valAttr != null)
                Value = (PassBlockVerdict)Enum.Parse(typeof(PassBlockVerdict), valAttr.Value, true);
        }
    }
}
