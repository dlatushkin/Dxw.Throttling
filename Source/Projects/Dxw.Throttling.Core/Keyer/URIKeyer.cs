namespace Dxw.Throttling.Core.Keyer
{
    using System.Net.Http;

    public class URIKeyer : IKeyer
    {
        public object GetKey(HttpRequestMessage request)
        {
            return request.RequestUri;
        }
    }
}
