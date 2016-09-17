namespace Dxw.Throttling.Asp.Keyers
{
    using Core;
    using Core.Configuration;
    using Core.Keyers;
    using Core.Logging;
    using System.Xml;

    public class IPKeyer : IKeyer<IAspArgs>, IXmlConfigurable
    {
        private ILog _log;

        private const string HttpContext = "MS_HttpContext";

        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        public object GetKey(IAspArgs args)
        {
            object ip = null;

            var request = args.Request;

            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic cntx = request.Properties[HttpContext];
                if (cntx != null)
                {
                    ip = cntx.Request.UserHostAddress;
                }
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    ip = remoteEndpoint.Address;
                }
            }

            return ip;
        }

        public void Configure(XmlNode node, IConfiguration context)
        {
            _log = context.Log;
            _log.Log(LogLevel.Debug, string.Format("Configuring keyer of type '{0}'", GetType().FullName));
        }
    }
}
