namespace Dxw.Throttling.Owin.Keyers
{
    using Core.Keyers;
    using Microsoft.Owin;
    using System.Net.Http;

    public class IPKeyer : IKeyer
    {
        private const string HttpContext = "MS_OwinContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        public object GetKey(object context)
        {
            object ip = null;

            var owinContext = context as IOwinContext;
            if (owinContext != null)
                ip = owinContext.Request.RemoteIpAddress;

            return ip;
        }
    }
}
