namespace Dxw.Throttling.Core
{
    using System.Xml;
    using Configuration;

    public interface IXmlConfigurable
    {
        void Configure(XmlNode node, IConfiguration context);
    }

    public interface IXmlConfigurable<T>
    {
        void Configure(XmlNode node, IConfiguration<T> context);
    }
}
