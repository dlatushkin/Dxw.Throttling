namespace Dxw.Throttling.Core.Keyer
{
    using System.Net.Http;

    public class URIMethodKeyer : IKeyer
    {
        public object GetKey(HttpRequestMessage request)
        {
            return new { request.RequestUri, request.Method };
        }
    }
}
