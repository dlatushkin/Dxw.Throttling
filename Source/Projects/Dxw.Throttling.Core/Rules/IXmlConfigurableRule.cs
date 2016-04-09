namespace Dxw.Throttling.Core.Rules
{
    using System.Xml;

    public interface IXmlConfigurableRule
    {
        void Configure(XmlNode node);
    }
}
