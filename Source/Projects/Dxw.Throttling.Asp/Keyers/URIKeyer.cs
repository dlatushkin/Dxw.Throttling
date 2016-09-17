namespace Dxw.Throttling.Asp.Keyers
{
    using System;
    using System.Xml;
    using Core;
    using Core.Keyers;
    using Core.Configuration;

    public class URIKeyer : IKeyer<IAspArgs>, IXmlConfigurable
    {
        public void Configure(XmlNode node, IConfiguration context)
        {
            throw new NotImplementedException();
        }

        public object GetKey(IAspArgs args)
        {
            return args.Request.RequestUri;
        }
    }
}
