namespace Dxw.Throttling.Owin.Keyers
{
    using Microsoft.Owin;

    using Core.Keyers;
    using Core.Exceptions;

    public class IPKeyer : IKeyer<IOwinContext>
    {
        private const string HttpContext = "MS_OwinContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        public object GetKey(IOwinContext owinContext)
        {
            if (owinContext == null)
                throw new ThrottlingRuleException("Context should be an instance of IOwinContext.");

            var owinRequest = owinContext.Request;
            if (owinRequest == null)
                throw new ThrottlingRuleException("Context should contain an instance of IOwinRequest.");

            return owinRequest.RemoteIpAddress;
        }
    }
}
