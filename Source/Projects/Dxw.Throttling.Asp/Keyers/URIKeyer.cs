namespace Dxw.Throttling.Asp.Keyers
{
    using System.Net.Http;

    using Core.Keyers;
    using Core.Exceptions;

    public class URIKeyer : IKeyer<HttpRequestMessage>
    {
        public object GetKey(HttpRequestMessage context)
        {
            var request = context as HttpRequestMessage;
            if (request != null)
                return request.RequestUri;

            throw new ThrottlingRuleException("Nor OwinRequest neither HttpRequestMessage are set in context argument");
        }
    }
}
