namespace Dxw.Throttling.Core.Keyer
{
    using Microsoft.Owin;
    using System.Net.Http;

    public interface IRequestContext
    {
        IOwinRequest OwinRequest { get; }
        HttpRequestMessage HttpRequestMessage { get; }
    }

    public class RequestContext : IRequestContext
    {
        private readonly IOwinRequest _owinRequest;
        private readonly HttpRequestMessage _httpRequestMessage;

        public RequestContext(IOwinRequest owinRequest, HttpRequestMessage httpRequestMessage)
        {
            _owinRequest = owinRequest;
            _httpRequestMessage = httpRequestMessage;
        }

        public IOwinRequest OwinRequest { get { return _owinRequest; } }

        public HttpRequestMessage HttpRequestMessage { get { return _httpRequestMessage; } }
    }
}
