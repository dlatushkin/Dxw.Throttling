namespace Dxw.Throttling.Core.Keyer
{
    using System.Net.Http;

    public class URIMethodKeyer : IKeyer
    {
        public object GetKey(IRequestContext context)
        {
            var owinContext = context.OwinRequest;
            if (owinContext != null)
                return owinContext.Uri;

            var request = context.HttpRequestMessage;
            if (request != null)
                return request.RequestUri;

            throw new ThrottlingRuleException("");

            return new { request.RequestUri, request.Method };
        }
    }
}
