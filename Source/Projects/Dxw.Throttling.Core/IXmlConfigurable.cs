namespace Dxw.Throttling.Core
{
    using System.Xml;

    public interface IXmlConfigurable
    {
        void Configure(XmlNode node);
    }
}
