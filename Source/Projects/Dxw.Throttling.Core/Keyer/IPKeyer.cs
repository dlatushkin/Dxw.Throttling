namespace Dxw.Throttling.Core.Keyer
{
    using System.Net.Http;

    public class IPKeyer : IKeyer
    {
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        public object GetKey(HttpRequestMessage request)
        {
            object ip = null;

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
    }
}
