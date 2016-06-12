namespace Dxw.Throttling.Asp.Keyers
{
    using System.Net.Http;

    using Core.Keyers;

    public class URIMethodKeyer : IKeyer
    {
        public object GetKey(object context)
        {
            var request = context as HttpRequestMessage;
            if (request != null)
                return request.RequestUri;

            return new { request.RequestUri, request.Method };
        }
    }
}
