namespace Dxw.Throttling.Asp
{
    using System.Net.Http;

    using Core;

    public class AspArgs : IAspArgs
    {
        public EventPhase Phase { get; set; }

        public HttpRequestMessage Request { get; set; }

        public HttpResponseMessage Response { get; set; }
    }
}
