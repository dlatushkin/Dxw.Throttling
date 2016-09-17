namespace Dxw.Throttling.Owin.Keyers
{
    using Core;
    using Core.Configuration;
    using Core.Keyers;
    using Core.Logging;
    using System.Xml;

    public class IPKeyer : IKeyer<IOwinArgs>, IXmlConfigurable
    {
        private ILog _log;

        private const string HttpContext = "MS_OwinContext";

        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        public object GetKey(IOwinArgs owinArgs)
        {
            return owinArgs.OwinContext.Request.RemoteIpAddress;
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            _log = context.Log;
            _log.Log(LogLevel.Debug, string.Format("Configuring keyer of type '{0}'", GetType().FullName));
        }
    }
}
