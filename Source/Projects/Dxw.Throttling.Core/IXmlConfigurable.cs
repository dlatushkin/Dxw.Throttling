namespace Dxw.Throttling.Core
{
    using System.Xml;
    using Configuration;

    public interface IXmlConfigurable
    {
        void Configure(XmlNode node, IConfiguration context);
    }

    public interface IXmlConfigurable<TArg, TRes>
    {
        void Configure(XmlNode node, IConfiguration<TArg, TRes> context);
    }
}
