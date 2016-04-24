namespace Dxw.Throttling.Core.Keyer
{
    using Microsoft.Owin;
    using System.Net.Http;

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

            throw new ThrottlingRuleException("");

            return new { request.RequestUri, request.Method };
        }
    }
}
