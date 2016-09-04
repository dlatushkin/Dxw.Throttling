namespace Dxw.Throttling.Core.Rules.Constant
{
    using System;
    using System.Xml;
    using Dxw.Throttling.Core.Configuration;

    public class ConstantPassBlockRule<TArg> : ConstantRule<TArg, PassBlockVerdict>
    {
        public override void Configure(XmlNode node, IConfiguration context)
        {
            base.Configure(node, context);

            var valAttr = node.Attributes["value"];
            if (valAttr != null)
                Value = (PassBlockVerdict)Enum.Parse(typeof(PassBlockVerdict), valAttr.Value, true);
        }
    }
}
