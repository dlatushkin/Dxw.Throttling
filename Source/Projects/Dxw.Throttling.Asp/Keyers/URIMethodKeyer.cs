namespace Dxw.Throttling.Asp.Keyers
{
    using System.Net.Http;

    using Microsoft.Owin;

    using Core.Keyer;

    public class URIMethodKeyer : IKeyer
    {
        public object GetKey(object context)
        {
            var owinContext = context as OwinRequest;
            if (owinContext != null)
                return owinContext.Uri;

            var request = context as HttpRequestMessage;
            if (request != null)
                return request.RequestUri;

            return new { request.RequestUri, request.Method };
        }
    }
}
