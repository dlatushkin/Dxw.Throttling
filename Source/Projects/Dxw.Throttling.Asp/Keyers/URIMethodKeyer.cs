namespace Dxw.Throttling.Asp.Keyers
{
    using System.Net.Http;

    using Core.Keyers;

    public class URIMethodKeyer : IKeyer<HttpRequestMessage>
    {
        public object GetKey(HttpRequestMessage request)
        {
            if (request != null)
                return request.RequestUri;

            return new { request.RequestUri, request.Method };
        }
    }
}
