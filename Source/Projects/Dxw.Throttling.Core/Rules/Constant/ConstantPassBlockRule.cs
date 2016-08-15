namespace Dxw.Throttling.Core.Rules.Constant
{
    using System;
    using System.Xml;
    using Dxw.Throttling.Core.Configuration;

    public class ConstantPassBlockRule<TArg> : ConstantRule<PassBlockVerdict, TArg>, IXmlConfigurable
    {
        public void Configure(XmlNode node, IConfiguration context)
        {
            var valAttr = node.Attributes["value"];
            if (valAttr != null)
            {
                PassBlockVerdict val;
                Enum.TryParse(valAttr.Value, out val);
            }
        }
    }
}
