namespace Dxw.Throttling.Asp.Keyers
{
    using System.Net.Http;

    using Core.Keyers;

    public class URIMethodKeyer : IKeyer<IAspArgs>
    {
        public object GetKey(IAspArgs args)
        {
            var request = args.Request;
            return new { request.RequestUri, request.Method };
        }
    }
}
