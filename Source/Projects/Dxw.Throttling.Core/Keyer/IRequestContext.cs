namespace Dxw.Throttling.Core.Keyer
{
    using Microsoft.Owin;
    using System.Net.Http;

    public enum RequestPhase { Before, After }

    public interface IRequestContext
    {
        RequestPhase Phase { get; }
        IOwinRequest OwinRequest { get; }
        HttpRequestMessage HttpRequestMessage { get; }
    }

    public class RequestContext : IRequestContext
    {
        private readonly RequestPhase _phase;
        private readonly IOwinRequest _owinRequest;
        private readonly HttpRequestMessage _httpRequestMessage;

        public RequestContext(RequestPhase phase, IOwinRequest owinRequest, HttpRequestMessage httpRequestMessage)
        {
            _phase = phase;
            _owinRequest = owinRequest;
            _httpRequestMessage = httpRequestMessage;
        }

        public IOwinRequest OwinRequest => _owinRequest;

        public HttpRequestMessage HttpRequestMessage => _httpRequestMessage;

        public RequestPhase Phase => _phase;
    }
}
