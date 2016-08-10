﻿namespace Dxw.Throttling.Asp.Keyers
{
    using Core.Keyers;

    public class IPKeyer : IKeyer<IAspArgs>
    {
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
    }
}
