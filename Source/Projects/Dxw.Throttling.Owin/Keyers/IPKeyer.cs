namespace Dxw.Throttling.Owin.Keyers
{
    using Core.Keyers;

    public class IPKeyer : IKeyer<IOwinArgs>
    {
        private const string HttpContext = "MS_OwinContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        public object GetKey(IOwinArgs owinArgs)
        {
            return owinArgs.OwinContext.Request.RemoteIpAddress;
        }
    }
}
