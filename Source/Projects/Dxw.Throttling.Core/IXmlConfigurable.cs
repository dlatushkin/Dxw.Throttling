namespace Dxw.Throttling.Core
{
    using System.Xml;
    using Configuration;

    public interface IXmlConfigurable
    {
        void Configure(XmlNode node, IConfiguration context);
    }

    public interface IXmlConfigurable<T, TArg>
    {
        void Configure(XmlNode node, IConfiguration<T, TArg> context);
    }
}
