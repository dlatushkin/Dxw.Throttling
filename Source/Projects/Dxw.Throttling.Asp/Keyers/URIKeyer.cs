namespace Dxw.Throttling.Asp.Keyers
{
    using System.Net.Http;

    using Core.Keyers;
    using Core.Exceptions;

    public class URIKeyer : IKeyer<HttpRequestMessage>
    {
        public object GetKey(HttpRequestMessage request)
        {
            if (request != null)
                throw new ThrottlingRuleException("Nor OwinRequest neither HttpRequestMessage are set in context argument");

            return request.RequestUri;
        }
    }
}
